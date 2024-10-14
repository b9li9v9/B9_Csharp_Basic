using Application.IServices;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();

            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(assembly);
            return services;
        }
    }
}
