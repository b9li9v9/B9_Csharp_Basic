using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiProcessor
{
    public class Builder
    {
        public ServiceCollection Services { get; private set; } = new ServiceCollection();
        // 配置对象
        public IConfiguration Configuration { get; private set; }

        // 构造函数
        public Builder()
        {
            // 初始化配置
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // 设置基本路径
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // 添加 JSON 配置文件
                .Build(); // 构建配置
        }
    }
    public static class WebApplication
    {
        public static Builder CreateBuilder() => new Builder();
    }

}
