using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitHelper.Configuration;
using RabbitHelper.IServices;
using RabbitHelper.Services;
namespace ApiProcessor
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddService(this IServiceCollection services)
        {
            // 注册 RabbitMQ Sender
            services.AddLogging();
            services.AddSingleton<IConsumer, Consumer>();
            return services;
        }

        internal static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var consumerRabbitConfiguration = configuration.Bind<ConsumerConfiguration>();
            services.AddSingleton(consumerRabbitConfiguration);
            var producerRabbitConfiguration = configuration.Bind<ProducerConfiguration>();
            services.AddSingleton(producerRabbitConfiguration);
            return services;
        }

        private static T Bind<T>(this IConfiguration configuration) where T : new()
        {
            var section = configuration.GetSection(typeof(T).Name); // 使用类名作为节名
            var instance = new T();

            foreach (var property in typeof(T).GetProperties())
            {
                var value = section[property.Name]; // 获取配置节中的属性值
                if (value != null && property.CanWrite)
                {
                    var convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(instance, convertedValue);
                }
            }

            return instance;
        }



        //private static RabbitMQConfiguration BindRabbitMQConfiguration(this IConfiguration configuration)
        //{
        //    // 获取 RabbitMQ 配置节
        //    var rabbitMqSection = configuration.GetSection(nameof(RabbitMQConfiguration));

        //    // 将配置节转换为字典
        //    var rabbitMqDictionary = rabbitMqSection.GetChildren()
        //                                            .ToDictionary(x => x.Key, x => x.Value);

        //    // 通过反射创建 RabbitMQConfiguration 的实例
        //    var rabbitMqConfig = Activator.CreateInstance(typeof(RabbitMQConfiguration));

        //    // 遍历字典，使用反射给类的属性赋值
        //    foreach (var kvp in rabbitMqDictionary)
        //    {
        //        var property = rabbitMqConfig.GetType().GetProperty(kvp.Key);

        //        // 确保该属性存在并且是可写的
        //        if (property != null && property.CanWrite)
        //        {
        //            // 根据属性类型转换字典中的值
        //            var convertedValue = Convert.ChangeType(kvp.Value, property.PropertyType);
        //            property.SetValue(rabbitMqConfig, convertedValue);
        //        }
        //    }

        //    return (RabbitMQConfiguration)rabbitMqConfig;
        //}

    }
}
