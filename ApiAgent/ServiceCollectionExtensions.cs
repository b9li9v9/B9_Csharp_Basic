using Microsoft.Extensions.Options;
using RabbitHelper.Configuration;
using RabbitHelper.IServices;
using RabbitHelper.Services;


namespace ApiAgent
{
    public static class ServiceCollectionExtensions
    {
        internal static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {


            return app;
        }

        internal static IServiceCollection AddService(this IServiceCollection services)
        {

            // 注册 RabbitMQ Sender
            services.AddSingleton<IProducer, Producer>();
            return services;
        }

        internal static ProducerConfiguration AddRabbitConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var rabbitConfiguration = configuration.GetSection(nameof(ProducerConfiguration));
            services.Configure<ProducerConfiguration>(rabbitConfiguration);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<ProducerConfiguration>>().Value);
            return rabbitConfiguration.Get<ProducerConfiguration>();
        }


    }
}
