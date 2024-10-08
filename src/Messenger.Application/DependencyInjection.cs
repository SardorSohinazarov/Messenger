using Messenger.Application.DataTransferObjects;
using Messenger.Application.Helpers;
using Messenger.Application.Services.Auth;
using Messenger.Application.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.Configure<SmtpSettings>(options =>
            {
                options.Host = configuration["SmtpSettings:Host"];
                options.Port = int.Parse(configuration["SmtpSettings:Port"]);
                options.EnableSsl = bool.Parse(configuration["SmtpSettings:EnableSsl"]);
                options.Username = configuration["SmtpSettings:Username"];
                options.Password = configuration["SmtpSettings:Password"];
            });

            services.AddScoped<IEmailService, EmailService>();


            return services;
        }
    }
}
