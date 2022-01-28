using System;
using System.Net;
using System.Net.Mail;
using BLL.Abstractions.Interfaces;
using Core;

namespace BLL.Services
{
    public class EmailService : IEmailService
    {

        // private readonly AppSettings _appSettings;
        //
        // public EmailService(AppSettings appSettings)
        // {
        //     _appSettings = appSettings;
        // }
        public async void SendingEmailOnRegistration(User user)
        {
            SmtpClient Client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential()
                {
                    UserName = "msl.corpor@gmail.com".Trim(),
                    Password = "21918111".Trim()
                }
            };
            MailAddress FromEmail = new MailAddress("msl.corpor@gmail.com", "MSL corp.");

            MailAddress ToEmail = new MailAddress(user.Email);

            MailMessage Message = new MailMessage()
            {
                From = FromEmail,
                Subject = "Messenger",
                Body = string.Concat(
                    "You have successfully created an account!\n",
                    $"Your login - {user.Nickname}\n",
                    $"Your password - {user.Password}\n",
                    $"If you have some questions, write an email on msl.corpor@gmail.com"
                )
            };
            Message.To.Add(ToEmail);
            Client.EnableSsl = true;
            await Client.SendMailAsync(Message);
        }
    }
}