# Messenger Project

Bu loyiha real vaqtda chat funksiyalarini taqdim etadigan Messenger ilovasidir. Ilova to'liq **Clean Architecture** prinsiplari asosida qurilgan bo'lib, **.NET** platformasida ishlaydi. 

## Tuzilishi

Ilovaning tahminiy tuzilishi quyidagicha:
```
Directory structure:
└── SardorSohinazarov-Messenger/
    ├── README.md
    ├── Messenger.sln
    └── src/
        ├── Messenger.Api/
        │   ├── Messenger.Api.csproj
        │   ├── Messenger.Api.http
        │   ├── Program.cs
        │   ├── appsettings.json
        │   ├── Controllers/
        │   │   ├── AuthController.cs
        │   │   ├── ChatUsersController.cs
        │   │   ├── ChatsController.cs
        │   │   ├── MessagesController.cs
        │   │   ├── ProfilesController.cs
        │   │   └── UsersController.cs
        │   ├── Extensions/
        │   │   └── ServiceCollectionExtensions.cs
        │   ├── Hubs/
        │   │   └── ChatHub.cs
        │   ├── Middlewares/
        │   │   └── ExceptionHandlingMiddleware.cs
        │   └── Properties/
        │       └── launchSettings.json
        ├── Messenger.Application/
        │   ├── DependencyInjection.cs
        │   ├── Messenger.Application.csproj
        │   ├── Common/
        │   │   └── MappingProfiles.cs
        │   ├── Extensions/
        │   │   └── QueryableExtensions.cs
        │   ├── Helpers/
        │   │   ├── PasswordHasher/
        │   │   │   ├── IPasswordHasher.cs
        │   │   │   └── PasswordHasher.cs
        │   │   └── UserContext/
        │   │       ├── IUserContextService.cs
        │   │       └── UserContextService.cs
        │   ├── Models/
        │   │   ├── DataTransferObjects/
        │   │   │   ├── Auth/
        │   │   │   │   ├── EmailConfirmationDto.cs
        │   │   │   │   ├── LoginDto.cs
        │   │   │   │   ├── RefreshTokenDto.cs
        │   │   │   │   ├── RegisterDto.cs
        │   │   │   │   ├── TokenDto.cs
        │   │   │   │   ├── Google/
        │   │   │   │   │   ├── GoogleLoginDto.cs
        │   │   │   │   │   ├── GoogleOAuthOptions.cs
        │   │   │   │   │   └── GoogleUserInfo.cs
        │   │   │   │   └── UserProfiles/
        │   │   │   │       ├── UserProfile.cs
        │   │   │   │       └── UserProfileModificationDto.cs
        │   │   │   ├── ChatUsers/
        │   │   │   │   ├── ChatUserDto.cs
        │   │   │   │   └── ChatUserViewModel.cs
        │   │   │   ├── Chats/
        │   │   │   │   ├── ChannelCreationDto.cs
        │   │   │   │   ├── ChatDetailsViewModel.cs
        │   │   │   │   ├── ChatInviteLinkViewModel.cs
        │   │   │   │   ├── ChatModificationDto.cs
        │   │   │   │   ├── ChatViewModel.cs
        │   │   │   │   ├── ChatsPaginationSelectionDto.cs
        │   │   │   │   ├── GroupCreationDto.cs
        │   │   │   │   └── UsersFilterModel.cs
        │   │   │   ├── CommonOptions/
        │   │   │   │   ├── JwtSettings.cs
        │   │   │   │   └── SmtpSettings.cs
        │   │   │   ├── Filters/
        │   │   │   │   └── ChatFilter.cs
        │   │   │   ├── Messages/
        │   │   │   │   ├── MessageCreationDto.cs
        │   │   │   │   ├── MessageModificationDto.cs
        │   │   │   │   ├── MessageViewModel.cs
        │   │   │   │   ├── MessagesPaginationSelectionByChatDto.cs
        │   │   │   │   └── MessagesPaginationSelectionDto.cs
        │   │   │   └── Users/
        │   │   │       └── UserViewModel.cs
        │   │   ├── Pagination/
        │   │   │   ├── PaginationMetadata.cs
        │   │   │   ├── PaginationParam.cs
        │   │   │   └── Cursor/
        │   │   │       ├── CursorPaginationMetadata.cs
        │   │   │       └── CursorPaginationParam.cs
        │   │   └── Results/
        │   │       └── Result.cs
        │   ├── Services/
        │   │   ├── Auth/
        │   │   │   ├── AuthService.cs
        │   │   │   ├── IAuthService.cs
        │   │   │   └── Google/
        │   │   │       ├── GoogleAuthService.cs
        │   │   │       └── IGoogleAuthService.cs
        │   │   ├── ChatUsers/
        │   │   │   ├── ChatUserService.cs
        │   │   │   └── IChatUserService.cs
        │   │   ├── Chats/
        │   │   │   ├── ChatService.cs
        │   │   │   └── IChatService.cs
        │   │   ├── Email/
        │   │   │   ├── EmailService.cs
        │   │   │   └── IEmailService.cs
        │   │   ├── Messages/
        │   │   │   ├── IMessagesService.cs
        │   │   │   └── MessageService.cs
        │   │   ├── Profiles/
        │   │   │   ├── IProfileService.cs
        │   │   │   └── ProfileService.cs
        │   │   ├── Token/
        │   │   │   ├── ITokenService.cs
        │   │   │   └── TokenService.cs
        │   │   └── Users/
        │   │       ├── IUserService.cs
        │   │       └── UserService.cs
        │   └── Validators/
        │       ├── Auth/
        │       │   ├── EmailConfirmationDtoValidator.cs
        │       │   ├── LoginDtoValidator.cs
        │       │   ├── RefreshTokenDtoValidator.cs
        │       │   ├── RegisterDtoValidator.cs
        │       │   └── UserProfileModificationDtoValidator.cs
        │       ├── Chats/
        │       │   ├── ChannelCreationDtoValidator.cs
        │       │   ├── ChatModificationDtoValidator.cs
        │       │   └── GroupCreationDtoValidator.cs
        │       └── Messages/
        │           ├── MessageCreationDtoValidator.cs
        │           └── MessageModificationDtoValidator.cs
        ├── Messenger.Domain/
        │   ├── Messenger.Domain.csproj
        │   ├── Common/
        │   │   ├── BaseEntity.cs
        │   │   ├── IAuditable.cs
        │   │   └── ISoftDeletable.cs
        │   ├── Entities/
        │   │   ├── Chat.cs
        │   │   ├── LinkTables.cs
        │   │   ├── Message.cs
        │   │   ├── Update.cs
        │   │   └── User.cs
        │   ├── Enums/
        │   │   ├── EChatType.cs
        │   │   └── ELoginProvider.cs
        │   └── Exceptions/
        │       ├── ForbiddenException.cs
        │       └── NotFoundException.cs
        └── Messenger.Infrastructure/
            ├── DependencyInjection.cs
            ├── Messenger.Infrastructure.csproj
            ├── Migrations/
            ...
            └── Persistence/
                ├── MessengerDbContext.cs
                └── Configurations/
                    ├── ChatConfiguration.cs
                    ├── ChatUserConfiguration.cs
                    ├── MessageConfiguration.cs
                    └── UserConfiguration.cs
```
