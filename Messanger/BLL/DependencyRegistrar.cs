using BLL.Abstractions.Interfaces;
using BLL.Services;
using Core.Models;
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
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<Session>();
            services.AddScoped<Actions>();
            DAL.DependencyRegistrar.ConfigureServices(services);
        }
    }
}