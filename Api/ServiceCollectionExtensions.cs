using Api.Permissions;
using Application.IServices;
using Common.Requests.Token;
using Common.Responses.Token;
using Infrastructure.Constant;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
namespace Api
{
    public static class ServiceCollectionExtensions
    {
        internal static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {

            using var serviceScope = app.ApplicationServices.CreateScope();

            var seeders = serviceScope.ServiceProvider.GetServices<ApplicationDbSeeeder>();

            foreach (var seeder in seeders)
            {
                seeder.SeedDatabaseAsync().GetAwaiter().GetResult();
            }
            return app;
        }

        internal static IApplicationBuilder GetAdminToken(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var _tokenService = serviceScope.ServiceProvider.GetService<ITokenService>();
            var _logger = serviceScope.ServiceProvider.GetService<ILogger<CreateTokenResponse>>();
            
            var _createTokenRequest = new CreateTokenRequest(){Email = AppCredentials.AdminEmail,Password = AppCredentials.AdminPassword};
            var _createTokenResponse = _tokenService.CreateTokenAsync(_createTokenRequest).GetAwaiter().GetResult();            
            _logger.LogWarning($"Bearer {_createTokenResponse.ResponseData.Token}");
            return app;
        }
        internal static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            // 使用 SwaggerGen 服务生成 API 文档
            services.AddSwaggerGen(options =>
            {
                // 定义名为 "Bearer" 的安全架构 (用于令牌认证)
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization", // 用于授权的头部名称
                    In = ParameterLocation.Header, // 安全架构的位置 (在头部)
                    Type = SecuritySchemeType.ApiKey, // 安全架构的类型 (API 密钥)
                    Scheme = "Bearer", // 架构方案名称 (与头部名称一致)
                    BearerFormat = "JWT", // 令牌格式 (JSON Web Token)
                    Description = "输入您的 Bearer 令牌，格式为 - Bearer {您的令牌在此处} 以访问此 API", // 清晰的解释
                });

                // 强制使用定义的 Bearer 架构进行安全验证
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme  // 引用之前定义的安全架构
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer", // 一致的架构方案名称
                            Name = "Bearer", // 一致的名称
                            In = ParameterLocation.Header, // 一致的位置
                        },
                        new List<string>() // 空列表表示所有 API 端点都需要 Bearer 令牌
                    },
                });

                // 创建一个用于 API 版本 "v1" 的 Swagger 文档
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1", // API 版本
                    Title = "Basic API", // API 标题
                    License = new OpenApiLicense  // 许可证信息
                    {
                        Name = "MIT License", // 许可证名称
                        Url = new Uri("https://opensource.org/licenses/MIT") // 许可证 URL
                    }
                });
            });

            return services;
        }

        internal static AppConfiguration AddApplicationConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var applicationConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationConfiguration);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppConfiguration>>().Value);
            return applicationConfiguration.Get<AppConfiguration>();
        }

        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string secret)
        {

            var key = Encoding.ASCII.GetBytes(secret);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = false;
                    // 设置令牌验证的参数
                    bearer.TokenValidationParameters = new TokenValidationParameters 
                    {
                        // 启用签名密钥验证，确保令牌是由预期的密钥签名的
                        ValidateIssuerSigningKey = true,
                        // 设置用于验证签名的对称密钥
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        // 禁用发行者验证，这意味着不检查令牌的发行者
                        ValidateIssuer = false,
                        // 禁用受众验证，这意味着不检查令牌的受众
                        ValidateAudience = false,
                        // 设置允许的时间偏移为零，不允许时间差异
                        ClockSkew = TimeSpan.Zero 
                    };

                });

            services.AddAuthorization(options =>
            {
                foreach (var prop in typeof(AppPermissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
                {
                    var propertyValue = prop.GetValue(null);
                    if (propertyValue is not null)
                    {
                        options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(AppClaim.Permission, propertyValue.ToString()));
                    }
                }
            });

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            return services;
        }

        internal static IServiceCollection AddApplicationCors(this IServiceCollection services)
        {
            services.AddCors(o =>
            o.AddPolicy("Basic Cors", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            return services;
        }
    }
}
