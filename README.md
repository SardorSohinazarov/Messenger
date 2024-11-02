# Messenger Project

Bu loyiha real vaqtda chat funksiyalarini taqdim etadigan Messenger ilovasidir. Ilova to'liq **Clean Architecture** prinsiplari asosida qurilgan bo'lib, **.NET** platformasida ishlaydi. 

## Tuzilishi

Ilovaning tahminiy tuzilishi quyidagicha:
```
/src
├── /Messenger.Api                           // Ilovaning kirish nuqtasi (Web API)
│   ├── /Controllers                         // API controller'lari
│   │   ├── AuthController.cs                // Autentifikatsiya funksiyalari uchun controller
│   │   ├── ChatsController.cs               // Chatlar bilan ishlash uchun controller
│   │   ├── ChatUsersController.cs           // Chat foydalanuvchilari bilan ishlash uchun controller
│   │   └── MessagesController.cs            // Xabarlar bilan ishlash uchun controller
│   ├── /Extensions                          // Kengaytmalar
│   │   └── ServiceCollectionExtensions.cs   // Xizmatlarni ro'yxatga olish uchun kengaytma
│   ├── appsettings.json                     // Ilova sozlamalari (bazaga ulanish, secret kalitlar va boshqalar)
│   ├── Messenger.Api.http                   // HTTP so'rovlarini sinash uchun fayl
│   └── Program.cs                           // Ilovani boshlaydigan asosiy fayl (Main)
|
├── /Messenger.Infrastructure             // Tashqi xizmatlar bilan ishlash qismi
│   ├── /Persistence                      // Ma'lumotlar bilan ishlash
│   │   ├── /Configurations               // Entity konfiguratsiyasi (EF Core uchun)
│   │   │   ├── MessageConfiguration.cs   // Xabar entity konfiguratsiyasi
│   │   │   └── ...                       // Qo'shimcha konfiguratsiyalar
│   │   ├── /Repositories                 // Repository implementatsiyalari
│   │   │   ├── UserRepository.cs         // Foydalanuvchilar repository
│   │   │   ├── MessageRepository.cs      // Xabarlar repository
│   │   │   └── ...                       // Qo'shimcha repository'lar
│   └── ApplicationDbContext.cs          // Ma'lumotlar konteksti
|
├── /Messenger.Application                // Biznes qoidalari va xizmatlar
│   ├── /Common                           // Umumiy xizmatlar yoki yordamchi kodlar
│   │   └── MappingProfiles.cs            // AutoMapper profilari
│   ├── /DataTransferObjects                // DTO'lar uchun katalog
│   │   ├── /Auth                           // Autentifikatsiya bilan bog'liq DTO'lar
|   |   │   ├── /Google                     // Google autentifikatsiyasi bilan bog'liq DTO'lar
|   |   │   │   ├── GoogleLoginDto.cs       // Google login ma'lumotlari
|   |   │   │   ├── GoogleOAuthOptions.cs   // Google OAuth sozlamalari
|   |   │   │   └── GoogleUserInfo.cs       // Google foydalanuvchi ma'lumotlari
|   |   │   ├── /UserProfiles               // Foydalanuvchi profili bilan bog'liq DTO'lar
|   |   │   │   ├── UserProfile.cs          // Foydalanuvchi profili ma'lumotlari
|   |   │   │   ├── UserProfileModificationDto.cs // Foydalanuvchi profilini tahrirlash uchun DTO
|   |   │   │   ├── EmailConfirmationDto.cs // Elektron pochta tasdiqlash uchun DTO
|   |   │   │   ├── LoginDto.cs             // Login uchun DTO
|   |   │   │   ├── RefreshTokenDto.cs      // Refresh token uchun DTO
|   |   │   │   ├── RegisterDto.cs          // Ro'yxatdan o'tish uchun DTO
|   |   │   │   └── TokenDto.cs             // Token ma'lumotlari uchun DTO
|   |   ├── /Chats                          // Chat bilan bog'liq DTO'lar
|   |   │   ├── ChannelCreationDto.cs       // Kanal yaratish uchun DTO
|   |   │   ├── ChatDetailsViewModel.cs     // Chat tafsilotlari uchun ViewModel
|   |   │   ├── ChatInviteLinkViewModel.cs  // Chat taklif havolasi uchun ViewModel
|   |   │   ├── ChatModificationDto.cs      // Chatni tahrirlash uchun DTO
|   |   │   ├── ChatViewModel.cs            // Chat ko'rinishidagi ViewModel
|   |   │   └── GroupCreationDto.cs         // Guruh yaratish uchun DTO
|   |   ├── /ChatUsers                      // Chat foydalanuvchilari bilan bog'liq DTO'lar
|   |   │   ├── ChatUserDto.cs              // Chat foydalanuvchisi uchun DTO
|   |   │   └── ChatUserViewModel.cs        // Chat foydalanuvchi ko'rinishi uchun ViewModel
|   |   ├── /CommonOptions                  // Umumiy sozlamalar uchun katalog
|   |   │   ├── JwtSettings.cs              // JWT sozlamalari
|   |   │   └── SmtpSettings.cs             // SMTP sozlamalari
|   |   ├── /Filters                        // Filtrlash uchun katalog
|   |   │   └── ChatFilter.cs               // Chat filtri uchun DTO
|   |   ├── /Messages                       // Xabarlar bilan bog'liq DTO'lar
|   |   │   ├── MessageCreationDto.cs       // Xabar yaratish uchun DTO
|   |   │   ├── MessageModificationDto.cs   // Xabarni tahrirlash uchun DTO
|   |   │   └── MessageViewModel.cs         // Xabar ko'rinishi uchun ViewModel
|   |   └── /Users                          // Foydalanuvchilar bilan bog'liq DTO'lar
|   |       └── UserViewModel.cs            // Foydalanuvchi ko'rinishi uchun ViewModel
│   ├── /Extensions                          // Kengaytmalar
│   │   └── FluentValidationExtensions.cs    // Fluent Validation kengaytmalari
│   ├── /Helpers                             // Yordamchi xizmatlar
│   │   ├── /PasswordHasher                  // Parolni hashlash xizmati
│   │   │   ├── IPasswordHasher.cs           // Parol hashlash interfeysi
│   │   │   └── PasswordHasher.cs            // Parol hashlash implementatsiyasi
│   │   └── /UserContext                     // Foydalanuvchi konteksti xizmati
│   │       ├── IUserContextService.cs       // Foydalanuvchi konteksti interfeysi
│   │       └── UserContextService.cs        // Foydalanuvchi konteksti xizmati
│   ├── /Services                         // Biznes qoidalarini amalga oshiruvchi xizmatlar
│   │   ├── /Auth                            // Autentifikatsiya xizmatlari
│   │   │   ├── /Google                      // Google autentifikatsiyasi uchun xizmatlar
│   │   │   │   ├── GoogleAuthService.cs     // Google autentifikatsiyasi xizmati
│   │   │   │   └── IGoogleAuthService.cs    // Google autentifikatsiyasi interfeysi
│   │   │   ├── AuthService.cs               // Autentifikatsiya xizmati
│   │   │   └── IAuthService.cs              // Autentifikatsiya interfeysi
│   │   ├── /Chats                           // Chat bilan ishlash uchun xizmatlar
│   │   │   ├── ChatService.cs               // Chat xizmati
│   │   │   └── IChatService.cs              // Chat interfeysi
│   │   ├── /ChatUsers                       // Chat foydalanuvchilari bilan ishlash uchun xizmatlar
│   │   │   ├── ChatUserService.cs           // Chat foydalanuvchisi xizmati
│   │   │   └── IChatUserService.cs          // Chat foydalanuvchisi interfeysi
│   │   ├── /Email                           // Elektron pochta xizmati
│   │   │   ├── EmailService.cs              // Elektron pochta xizmati
│   │   │   └── IEmailService.cs             // Elektron pochta interfeysi
│   │   ├── /Messages                        // Xabarlar bilan ishlash uchun xizmatlar
│   │   │   ├── MessageService.cs            // Xabar xizmati
│   │   │   └── IMessageService.cs           // Xabar interfeysi
│   │   └── /Token                           // Token bilan ishlash uchun xizmatlar
│   │       ├── TokenService.cs              // Token xizmati
│   │       └── ITokenService.cs             // Token interfeysi
|   ├── /Validators                          // Validatsiya uchun xizmatlar
|   │   ├── LoginDtoValidator.cs             // Login qilish DTO uchun validatsiya
|   │   └── ...                              // Boshqa DTO'lar uchun validatsiyalar
│   └── Application.csproj               // Application loyihasining fayli
|
├── /Messenger.Domain                     // Ilovaning domen modeli (entity'lar, interfeyslar, biznes qoidalari)
│   ├── /Entities                         // Domain modellar (Entity'lar)
│   │   ├── User.cs                      // Foydalanuvchi modeli
│   │   ├── Message.cs                   // Xabar modeli
│   │   └── ...                          // Qo'shimcha domain entity'lar
│   ├── /Interfaces                       // Domen interfeyslar
│   │   ├── IUserRepository.cs            // Foydalanuvchilar repository interfeysi
│   │   ├── IMessageRepository.cs         // Xabarlar repository interfeysi
│   │   └── ...                          // Qo'shimcha interfeyslar
│   └── Domain.csproj                    // Domain loyihasining fayli

/tests                                   // Testlar (unit va integration testlar)
├── /UnitTests                           // Unit testlar
│   ├── UserServiceTests.cs               // Foydalanuvchilar xizmati uchun unit testlar
│   ├── ChatServiceTests.cs               // Chat xizmati uchun unit testlar
│   └── ...                               // Qo'shimcha unit testlar
└── /IntegrationTests                    // Integration testlar
     ├── ChatControllerTests.cs           // Chat controlleri uchun integration testlar
     └── ...                               // Qo'shimcha integration testlar

Messenger.sln                            // Ilovaning umumiy yechimi (solution)
```
