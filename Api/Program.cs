
using Infrastructure;
using Application;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            {
                builder.WebHost.UseUrls($"http://0.0.0.0:{5140}");
                builder.Services.AddApplicationCors();
                builder.Services.AddControllers();
                builder.Services.AddHttpContextAccessor();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddDatabase(builder.Configuration.GetConnectionString("DefaultConnection"));
                builder.Services.AddApplicationConfiguration(builder.Configuration);
                builder.Services.AddApplicationService();
                builder.Services.AddJwtAuthentication(builder.Configuration.GetSection("AppConfiguration").GetSection("Secret").Value);
                builder.Services.RegisterSwagger();

            }

            var app = builder.Build();
            { 
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors("Basic Cors");
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                //app.SeedDatabase();
                app.GetAdminToken();
            }

            app.Run();
        }
    }
}
