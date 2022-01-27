using System.IO;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messanger
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<App>().StartApp();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // E:\dotnet messanger\Messanger\PresentationLayer
            // E:\Dev\.NET bootcamp\messanger_dotnet_group_5\Messanger\PresentationLayer
            // C:\Users\aleks\Desktop\messanger_dotnet_group_5\Messanger\PresentationLayer
            var configuration = new ConfigurationBuilder()
                .SetBasePath(@"C:\Users\aleks\Desktop\messanger_dotnet_group_5\Messanger\PresentationLayer")
                .AddJsonFile(@"appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddScoped<App>();
            services.AddScoped<ConsoleInterface>();
            
            BLL.DependencyRegistrar.ConfigureServices(services);
        }
    }
}