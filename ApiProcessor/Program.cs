
using Common.ForwardMessage;
using Common.Requests.Token;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitHelper.Configuration;
using RabbitHelper.IServices;
using System.Text;

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


            int i = 0;
            await consumer.StartConsuming(async message =>
            {
                
                i++;
                Console.WriteLine(i);
                // 监听
                ForwardMessage deserializeMessage = JsonConvert.DeserializeObject<ForwardMessage>(message);
                Console.WriteLine($"Received message: {deserializeMessage.requestId}");
                Console.WriteLine($"Received message: {deserializeMessage.httpRequestInfo.httpRequestHeader.GetType()}");
                Console.WriteLine($"Received message: {deserializeMessage.httpRequestInfo.body}");

                // 调用server
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var requestData = deserializeMessage.httpRequestInfo.body;
                        var json = JsonConvert.SerializeObject(requestData);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        // 发送 GET 请求
                        HttpResponseMessage response = await client.PostAsync("http://localhost:5140"+ deserializeMessage.httpRequestInfo.pathString+"/"+ deserializeMessage.httpRequestInfo.userId, content);
                        Console.WriteLine("http://localhost:5140/" + deserializeMessage.httpRequestInfo.pathString + "/" + deserializeMessage.httpRequestInfo.userId);
                        response.EnsureSuccessStatusCode(); // 确保 HTTP 响应状态为 200

                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"请求错误: {e.Message}");
                    }
                }

                // 转发回RabbitHost  这里配置项，既有配置类又有回调地址  重叠了。
                //var producer = builder.Services.BuildServiceProvider().GetService<IProducer>();
                //var ProducerConfiguration = builder.Services.BuildServiceProvider().GetService<ProducerConfiguration>();
                //await producer.CreateModel(ProducerConfiguration.ExchangeName,
                //                       ProducerConfiguration.ExchangeType,
                //                       true,
                //                       deserializeMessage.httpRequestInfo.userId != null ? deserializeMessage.httpRequestInfo.userId : ProducerConfiguration.QueueName,
                //                       true,
                //                       false,
                //                       false,
                //                       deserializeMessage.httpRequestInfo.userId != null ? deserializeMessage.httpRequestInfo.userId : ProducerConfiguration.QueueName
                //                       );



            },10);
            Console.WriteLine($"outSide");

            Console.ReadKey();
            // 释放资源
            consumer.Dispose();
        }

            
    }
}
