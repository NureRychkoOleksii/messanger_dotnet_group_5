using System;
using DAL.Abstractions.Interfaces;
using DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ISerializationWorker, SerializationWorker>();
        }
    }
}