using Microsoft.EntityFrameworkCore;
using Web_MSL.Data;
using Web_MSL.Data.Abstractions;

namespace Web_MSL.Controllers;

public class DependencyRegistrar
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}