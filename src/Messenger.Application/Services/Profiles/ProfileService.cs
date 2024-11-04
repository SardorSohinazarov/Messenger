using Messenger.Application.DataTransferObjects.Auth.UserProfiles;
using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.Validators.Auth;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using AutoMapper;
using Messenger.Application.Helpers.UserContext;
using Messenger.Application.Services.Email;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ValidationException = FluentValidation.ValidationException;
using Messenger.Domain.Exceptions;
using Messenger.Application.Common.Results;

namespace Messenger.Application.Services.Profiles
{
    public class ProfileService : IProfileService
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public ProfileService(
            MessengerDbContext messengerDbContext,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            IUserContextService userContextService,
            IMapper mapper,
            IMemoryCache memoryCache)
        {
            _messengerDbContext = messengerDbContext;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _userContextService = userContextService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<Result<UserProfile>> GetUserProfileAsync()
        {
            var userId = _userContextService.GetCurrentUserId();

            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                throw new NotFoundException("User topilmadi.");

            var userProfile = _mapper.Map<UserProfile>(user);
            return Result<UserProfile>.Success(userProfile);
        }

        public async Task<Result<UserProfile>> GetUserProfileAsync(long userId)
        {
            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                throw new NotFoundException("User topilmadi.");

            var userProfile = _mapper.Map<UserProfile>(user);
            return Result<UserProfile>.Success(userProfile);
        }

        public async Task<Result<UserProfile>> UpdateUserProfileAsync(UserProfileModificationDto userProfileModificationDto)
        {
            var validator = new UserProfileModificationDtoValidator();
            var result = await validator.ValidateAsync(userProfileModificationDto);

            if (!result.IsValid)
                throw new ValidationException("Model update qilish uchun yaroqsiz", result.Errors);

            #region Optimallashtirish kerak : Telefon nomer yoki Email yoki UserName band qilinganmi yo'qmi tekshirish kerak 
            var userNameOrPhoneOrEmailExist = await _messengerDbContext.Users
                .AnyAsync(x => x.UserName == userProfileModificationDto.UserName
                            || x.Email == userProfileModificationDto.Email
                            || x.PhoneNumber == userProfileModificationDto.PhoneNumber);

            if (userNameOrPhoneOrEmailExist)
                throw new ValidationException("Telefon nomer yoki Email yoki UserName allaqachon band qilingan.");
            #endregion

            var userId = _userContextService.GetCurrentUserId();

            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            user = _mapper.Map(userProfileModificationDto, user);
            var entryEntity = _messengerDbContext.Users.Update(user);
            await _messengerDbContext.SaveChangesAsync();

            var userProfile = _mapper.Map<UserProfile>(entryEntity.Entity);
            return Result<UserProfile>.Success(userProfile);
        }


        public async Task<Result> DeleteUserProfileAsync()
        {
            var userId = _userContextService.GetCurrentUserId();

            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                throw new NotFoundException("User topilmadi.");

            var confirmationCode = new Random().Next(1000, 9999).ToString();

            var confirmationLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Auth/confirm-delete-profile?Email={Uri.EscapeDataString(user.Email)}&ConfirmationCode={confirmationCode}";
            var emailBody = new StringBuilder(@"""<!DOCTYPE html>
                <html lang=""uz"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 20px;
                        }
                        .container {
                            background-color: #ffffff;
                            border-radius: 5px;
                            padding: 20px;
                            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                            text-align: center; /* O'rtaga joylash uchun */
                        }
                        h1 {
                            color: #333333;
                        }
                        p {
                            color: #555555;
                            line-height: 1.6;
                        }
                        .button {
                            background-color: #007bff;
                            color: #ffffff;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 5px;
                            display: inline-block;
                            margin-top: 20px;
                        }
                        .code {
                            font-size: 36px; /* Tasdiqlash kodining o'lchami */
                            font-weight: bold; /* Katta harfda bo'lishi */
                            color: #007bff; /* Tasdiqlash kodi uchun rang */
                            padding: 20px;
                            border: 2px solid #007bff; /* Tasdiqlash kodi uchun chegara */
                            display: inline-block; /* Hujjatning o'ziga xos joyini saqlash */
                            margin-top: 30px; /* O'rta joylash uchun yuqoridan bo'shliq */
                        }
                        .footer {
                            margin-top: 30px;
                            font-size: 12px;
                            color: #888888;
                            text-align: center;
                        }
                    </style>
                </head>
                <body>

                <div class=""container"">
                    <h1>Hisobingizni tasdiqlash!</h1>
                    <p>Salom <strong>FOYDALANUVCHIISMI</strong>,</p>
                    <p>Biz xizmatlarimizdan foydalanganingiz uchun rahmat, o'chirishdan oldin elektron pochtangizni tasdiqlashingiz kerak.</p>
    
                    <p>Quyidagi tasdiqlash kodini kiriting:</p>
                    <div class=""code"">KOD</div> <!-- Tasdiqlash kodi -->
    
                    <p>Yoki tasdiqlash uchun quyidagi tugmani bosing:</p>
                    <a href=""LINK"" class=""button"">Tasdiqlash uchun boshing</a>
    
                    <p>Biz bilan qoling va ajoyib imkoniyatlardan foydalaning!</p>
                </div>

                <div class=""footer"">
                    <p>Yana bir savolingiz bo'lsa, biz bilan bog'laning.</p>
                    <p>Bizning manzilimiz: sardorstudent0618@gmail.com</p>
                </div>

                </body>
                </html>""");

            emailBody.Replace("KOD", confirmationCode); // Tasdiqlash kodi
            emailBody.Replace("LINK", confirmationLink); // Tasdiqlash linki
            emailBody.Replace("FOYDALANUVCHIISMI", user.FirstName); // Foydalanuvchi ismi

            _memoryCache.Set(
                key: userId,
                value: new EmailConfirmationDto()
                {
                    Email = user.Email,
                    ConfirmationCode = confirmationCode,
                },
                options: new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(3),
                }
                );

            await _emailService.SendEmailAsync(user.Email, user.FirstName, "Confirm Delete Account", emailBody.ToString());

            return Result.Success("Code emailga jo'natildi.");
        }

        //Todo email html codeni boshqa joydan o'qishligi kerak M:bazadan
        public async Task<Result<UserProfile>> ConfirmDeleteProfileAsync(EmailConfirmationDto emailConfirmationDto)
        {
            var validator = new EmailConfirmationDtoValidator();
            var result = await validator.ValidateAsync(emailConfirmationDto);

            if (!result.IsValid)
                throw new ValidationException("Model email tasdiqlash uchun yaroqsiz.", result.Errors);

            var userId = _userContextService.GetCurrentUserId();

            var emailConfiramtionCache = _memoryCache.Get(userId) as EmailConfirmationDto;
            if (emailConfiramtionCache == null)
                throw new ValidationException("Kodni yaroqlilik muddati tugagan.");

            if (emailConfiramtionCache.Email != emailConfirmationDto.Email)
                throw new ValidationException("Email xato.");

            if (emailConfiramtionCache.ConfirmationCode != emailConfirmationDto.ConfirmationCode)
                throw new ValidationException("Tasdiqlash kodi noto'g'ri.");

            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == emailConfirmationDto.Email);

            if (user is null)
                throw new NotFoundException("Foydalanuvchi topilmadi.");

            var entryEntity = _messengerDbContext.Users.Remove(user);
            await _messengerDbContext.SaveChangesAsync();

            var userProfile = _mapper.Map<UserProfile>(entryEntity.Entity);
            return Result<UserProfile>.Success(userProfile);
        }
    }
}
