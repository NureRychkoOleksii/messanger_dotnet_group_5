using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IEmailService
    {
        void SendingEmailOnRegistration(User user);
        void SendingEmailOnInviting(User user, string roomName);
    }
}