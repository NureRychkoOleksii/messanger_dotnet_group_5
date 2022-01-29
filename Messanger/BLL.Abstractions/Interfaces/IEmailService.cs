using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IEmailService
    {
        void SendingEmailOnRegistration(User user);
    }
}