using System;
using DAL.Abstractions.Interfaces;
using DAL.DataBase;
using DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            // services.AddScoped<ISerializationWorker, SerializationWorker>();
            
            services.AddDbContext<DALContext>(options =>
                options.UseSqlServer("DESKTOP-MPQ8K2S=ConnectionStrings:DefaultConnection"));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}