
using RabbitMQ.Client;
using RabbitHelper.Configuration;
using RabbitHelper.IServices;
using System.Text;
using Microsoft.Extensions.Logging;

namespace RabbitHelper.Services
{
    //1.用主题交换器，2,每个用户拥有一个队列 3.路由键消息分类
    public class Producer : IProducer, IDisposable
    {
        private readonly ProducerConfiguration _producerConfiguration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<Producer> _logger;

        public Producer(ProducerConfiguration producerConfiguration,ILogger<Producer> logger)
        {
            _producerConfiguration = producerConfiguration;
             _logger = logger;
            var factory = new ConnectionFactory
            {
                HostName = _producerConfiguration.HostName,
                UserName = _producerConfiguration.UserName,
                Password = _producerConfiguration.Password,
                Port = int.Parse(_producerConfiguration.Port)
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                // this.CreateModel(_rabbitMQConf.QueueName, _rabbitMQConf.ExchangeName, true, false, false, null).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                // 异常处理逻辑
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// 声明队列并绑定到交换机
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="type">交换机类型direct topic</param>
        /// <param name="exchangeDurable">是否持久化</param>
        /// <param name="queue">队列名称</param>
        /// <param name="queueDurable">是否持久化</param>
        /// <param name="exclusive">是否排他，仅限于此连接使用</param>
        /// <param name="autoDelete">是否自动删除</param>
        /// <param name="arguments">额外参数</param>
        public Task<bool> CreateModel(string exchange,
                                 string type,
                                 bool exchangeDurable,

                                 string queue,
                                 bool queueDurable,
                                 bool exclusive,
                                 bool autoDelete,
                                 
                                 string routingKey,
                                 IDictionary<string, object> arguments = null)
        {
            try
            {
                // 声明交换机
                _channel.ExchangeDeclare(exchange: exchange,
                                        type: type,
                                        durable: exchangeDurable);

                // 声明队列
                _channel.QueueDeclare(queue: queue,
                                      durable: queueDurable,
                                      exclusive: exclusive,
                                      autoDelete: autoDelete,
                                      arguments: arguments);

                // 将队列绑定到交换机
                _channel.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey);
                return Task.FromResult(true);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                return Task.FromResult(false);
            }
        }

        /// <summary> 
        /// 将消息发送到指定的队列或交换机。
        /// 此方法允许发送消息并配置消息的路由键和属性。
        /// </summary>
        /// <param name="_message">要发送的消息内容。</param>
        /// <param name="_routingKey">路由键，指示消息发送到的队列，默认为“defaultQueue”。</param>
        /// <param name="_exchange">要发送消息的交换机名称，默认为空，表示使用默认交换机。</param>
        /// <param name="_basicProperties">消息的基本属性，允许设置持久化等属性，默认为 null。</param>
        /// <returns>返回一个布尔值，指示消息是否成功发送。</returns>
        public async Task<bool> SendMessage(string _message,
                                string _routingKey,
                                string _exchange,
                                IBasicProperties _basicProperties = null)
        {
            if (!this._channel.IsOpen)
            {
                _logger.LogError($"this._channel.IsOpen:{this._channel.IsOpen}");
                return false;
            }
            _channel.ConfirmSelect();


            //// 将消息对象序列化为 JSON 字符串
            //var jsonMessage = JsonConvert.SerializeObject(message); message is class
            // 需要定义个传输格式

            //// 将 JSON 字符串转换为字节数组
            //var body = Encoding.UTF8.GetBytes(jsonMessage);

            // 将消息编码为字节数组
            var body = Encoding.UTF8.GetBytes(_message);

            // 设置持久化属性
            if (_basicProperties == null)
            {
                _basicProperties = _channel.CreateBasicProperties();
                _basicProperties.Persistent = true; // 例如，设置为持久化
            }

            // 发送消息到指定的交换机和队列（通过 routingKey）
            _channel.BasicPublish(exchange: _exchange,
                                  routingKey: _routingKey,
                                  basicProperties: _basicProperties,
                                  body: body);
            // 创建任务来等待确认，避免阻塞
            bool isConfirmed = await Task.Run(() => _channel.WaitForConfirms());
             
            // 等待确认
            if(!isConfirmed)
            {
                _logger.LogError($"isConfirmed:{isConfirmed}");
            }

            return isConfirmed;

        }
        public void Dispose()
        {
            if (_channel?.IsOpen == true)
            {
                _channel.Close();
            }

            if (_connection?.IsOpen == true)
            {
                _connection.Close();
            }
        }
    }

}