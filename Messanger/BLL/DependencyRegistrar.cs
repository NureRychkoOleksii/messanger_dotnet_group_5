using BLL.Abstractions.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomUsersService, RoomUsersService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUsersInvitationService, UserInvitationService>();
            services.AddScoped<Session>();
            DAL.DependencyRegistrar.ConfigureServices(services);
        }
    }
}