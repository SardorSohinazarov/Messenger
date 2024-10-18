using Messenger.Application.Common;
using Messenger.Application.Helpers.PasswordHasher;
using Messenger.Application.Helpers.UserContetx;
using Messenger.Application.Helpers.UserContext;
using Messenger.Application.Services.Auth;
using Messenger.Application.Services.Auth.Google;
using Messenger.Application.Services.Chats;
using Messenger.Application.Services.ChatUsers;
using Messenger.Application.Services.Email;
using Messenger.Application.Services.Messages;
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
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IChatUserService, ChatUserService>();
            services.AddScoped<IMessagesService, MessageService>();

            //helpers
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            return services;
        }
    }
}
