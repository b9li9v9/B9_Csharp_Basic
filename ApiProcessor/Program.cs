using Common.ForwardMessage;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitHelper.IServices;

namespace ApiProcessor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddService();
            builder.Services.AddConfiguration(builder.Configuration);
            
            // 获取 RabbitMqConsumer 实例并开始消费
            var consumer = builder.Services.BuildServiceProvider().GetService<IConsumer>();
            consumer.StartConsuming(message =>
            {
                //Console.WriteLine($"Received message: {message}"); // 处理消息
                //Console.WriteLine();
                //Console.WriteLine($"abc");
                ForwardMessage deserializeMessage = JsonConvert.DeserializeObject<ForwardMessage>(message);
                Console.WriteLine($"Received message: {deserializeMessage.requestId}");
                Console.WriteLine($"Received message: {deserializeMessage.httpRequestInfo.url}");
                Console.WriteLine($"Received message: {deserializeMessage.httpRequestInfo.body}");
            });
            Console.WriteLine($"out");

            Console.ReadKey();
            // 释放资源
            consumer.Dispose();
        }

            
    }
}
