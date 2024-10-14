
using RabbitHelper.IServices;
using System.Text;
using RabbitHelper.Configuration;
using RabbitMQ.Client;

using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;



namespace RabbitHelper.Services
{
    public class Consumer : IConsumer, IDisposable
    {
        private readonly IConnection _connection; // RabbitMQ 连接
        private readonly IModel _channel; // RabbitMQ 通道
        private readonly ConsumerConfiguration _consumerConfiguration; // RabbitMQ 配置信息
        private readonly ILogger<Consumer> _logger; // 日志记录器

        // 构造函数，接收 RabbitMQ 配置信息和日志记录器
        public Consumer(ConsumerConfiguration consumerConfiguration, ILogger<Consumer> logger)
        {
            _consumerConfiguration = consumerConfiguration ?? throw new ArgumentNullException(nameof(ConsumerConfiguration));
            _logger = logger;

            // 验证配置是否有效
            if (string.IsNullOrEmpty(_consumerConfiguration.HostName) ||
                string.IsNullOrEmpty(_consumerConfiguration.UserName) ||
                string.IsNullOrEmpty(_consumerConfiguration.Password) ||
                string.IsNullOrEmpty(_consumerConfiguration.ExchangeName))
            {
                throw new ArgumentException("RabbitMQ configuration is invalid.");
            }

            // 创建连接工厂并配置连接参数
            var factory = new ConnectionFactory
            {
                HostName = _consumerConfiguration.HostName,
                UserName = _consumerConfiguration.UserName,
                Password = _consumerConfiguration.Password,
                Port = int.Parse(_consumerConfiguration.Port)
            };
            _connection = factory.CreateConnection(); // 创建连接
            _channel = _connection.CreateModel(); // 创建通道

            // 声明交换机，使用 direct 类型
            _channel.ExchangeDeclare(exchange: _consumerConfiguration.ExchangeName, 
                                     type: _consumerConfiguration.ExchangeType,
                                     durable:true);

            // 声明队列并与交换机绑定，通过路由键指定消息
            _channel.QueueBind(queue: _consumerConfiguration.QueueName, 
                                exchange: _consumerConfiguration.ExchangeName, 
                                routingKey: _consumerConfiguration.RoutingKey); // 绑定路由键
        }

        // 开始消费消息，接收消息处理方法和预取数量
        public virtual async Task StartConsuming(Action<string> messageHandler, ushort prefetchCount = 1)
        {
            _channel.BasicQos(0, prefetchCount, false); // 设置预取数量

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async(model, ea) =>
            {
                var body = ea.Body.ToArray(); // 获取消息体
                var message = Encoding.UTF8.GetString(body); // 转换为字符串
                try
                {
                    _logger.LogInformation($"Received message: {message}"); // 记录接收到的消息
                    //messageHandler(message); // 处理消息
                    await Task.Run(() => messageHandler(message));
                    _channel.BasicAck(ea.DeliveryTag, false); // 手动确认消息处理
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message"); // 记录错误信息
                    _channel.BasicNack(ea.DeliveryTag, false, false); // 拒绝消息并选择不重新排队
                }
            };

            // 开始消费消息，使用绑定时生成的队列名称
            _channel.BasicConsume(queue: _consumerConfiguration.QueueName, autoAck: false, consumer: consumer);
        }

        // 释放资源
        public void Dispose()
        {
            _channel?.Close(); // 关闭通道
            _channel?.Dispose(); // 释放通道资源
            _connection?.Close(); // 关闭连接
            _connection?.Dispose(); // 释放连接资源
        }
    }
}
