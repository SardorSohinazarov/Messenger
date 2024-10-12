using Messenger.Application.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Messenger.Application.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IConfiguration configuration) 
            => _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

        public async Task SendEmailAsync(string to, string toDisplayName, string subject, string body)
        {
            using (var client = new SmtpClient())
            {
                client.Host = _smtpSettings.Host;// "smtp.gmail.com";
                client.Port = _smtpSettings.Port;// 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = _smtpSettings.EnableSsl;// true;
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                using (var message = new MailMessage(
                    from: new MailAddress(
                        address: _smtpSettings.Username,
                        displayName: _smtpSettings.DisplayName),
                    to: new MailAddress(
                        address: to,
                        displayName: toDisplayName)
                    ))
                {

                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
