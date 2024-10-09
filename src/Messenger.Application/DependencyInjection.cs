using Messenger.Application.DataTransferObjects;
using Messenger.Application.Helpers.PasswordHasher;
using Messenger.Application.Services.Auth;
using Messenger.Application.Services.Email;
using Messenger.Application.Services.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.Configure<SmtpSettings>(options =>
            {
                options.Host = configuration["SmtpSettings:Host"];
                options.Port = int.Parse(configuration["SmtpSettings:Port"]);
                options.EnableSsl = bool.Parse(configuration["SmtpSettings:EnableSsl"]);
                options.Username = configuration["SmtpSettings:Username"];
                options.Password = configuration["SmtpSettings:Password"];
            });

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
