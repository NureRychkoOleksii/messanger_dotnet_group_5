using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DAL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messanger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // var unitOfWork = new UnitOfWork();

            // await unitOfWork.UserRepository.Insert(new User()
            // {
            //     Email = "oleksii.rychko@nure.ua",
            //     Nickname = "Moonler",
            //     Password = "1234",
            // });
            // await unitOfWork.Save();
            
            // var user = await unitOfWork.UserRepository.Get();
            //
            // if (user != null)
            // {
            //     var firstOrDefault = user.FirstOrDefault();
            //     Console.WriteLine(firstOrDefault.Id);
            //     Console.WriteLine(firstOrDefault.Nickname);
            //     Console.WriteLine(firstOrDefault.Password);
            //     Console.WriteLine(firstOrDefault.Email);
            // }
            // Console.WriteLine();

            // var services = new ServiceCollection();
            // ConfigureServices(services);
            // var serviceProvider = services.BuildServiceProvider();
            // serviceProvider.GetService<App>().StartApp();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // E:\dotnet messanger\Messanger\PresentationLayer
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
            //services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddScoped<App>();

            services.AddScoped<ConsoleInterface>();
            
            BLL.DependencyRegistrar.ConfigureServices(services);
        }
    }
}