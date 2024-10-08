namespace Messenger.Application.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string toDisplayName, string subject, string body);
    }
}
