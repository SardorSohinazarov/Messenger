# Messenger Project

Bu loyiha real vaqtda chat funksiyalarini taqdim etadigan Messenger ilovasidir. Ilova to'liq **Clean Architecture** prinsiplari asosida qurilgan bo'lib, **.NET** platformasida ishlaydi. 

## Tuzilishi

Ilovaning tahminiy tuzilishi quyidagicha:
```
/src
├── /Messenger.Api                     // Ilovaning kirish nuqtasi (Web API)
│   ├── /Controllers                    // API controller'lari
│   │   ├── UsersController.cs          // Foydalanuvchilar bilan ishlash uchun controller
│   │   ├── ChatController.cs           // Chat funksiyalarini boshqarish uchun controller
│   │   └── ...                         // Qo'shimcha controller'lar
│   ├── /Hubs                           // SignalR hublari
│   │   ├── ChatHub.cs                  // Chat hub'i
│   │   └── ...                         // Qo'shimcha hub'lar
│   ├── Program.cs                      // Ilovani boshlaydigan asosiy fayl (Main)
│   ├── appsettings.json                // Ilova sozlamalari (bazaga ulanish, secret kalitlar va boshqalar)
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
├── /Messenger.Application                // Biznes qoidalari va xizmatlar
│   ├── /DataTransferObjects              // DTO lar uchun katalog
│   │   ├── UserDto.cs                   // Foydalanuvchi ma'lumotlari uchun DTO
│   │   ├── ChatDto.cs                   // Chat ma'lumotlari uchun DTO
│   │   ├── MessageDto.cs                // Xabar ma'lumotlari uchun DTO
│   │   └── ...                          // Qo'shimcha DTO'lar
│   ├── /Services                         // Biznes qoidalarini amalga oshiruvchi xizmatlar
│   │   ├── UserService.cs               // Foydalanuvchilar bilan ishlovchi xizmat
│   │   ├── ChatService.cs               // Chat xizmati
│   │   └── ...                          // Qo'shimcha xizmatlar
│   ├── /Common                           // Umumiy xizmatlar yoki yordamchi kodlar
│   │   └── MappingProfiles.cs            // AutoMapper profilari
│   └── Application.csproj               // Application loyihasining fayli
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
