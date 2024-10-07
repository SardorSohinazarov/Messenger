using Messenger.Domain.Common;

namespace Messenger.Domain.Entities
{
    public class User : Auditable<long>
    {
        public string FirstName { get; set; }  // Foydalanuvchi ismi
        public string LastName { get; set; }  // Ixtiyoriy. Foydalanuvchi familiyasi
        public string UserName { get; set; }  // Ixtiyoriy. Foydalanuvchi foydalanuvchi nomi
        public string PhoneNumber { get; set; }  // Foydalanuvchi telefon raqami
        public string Email { get; set; }  // Ixtiyoriy. Foydalanuvchi elektron pochta manzili
        public string LanguageCode { get; set; }  // Ixtiyoriy. Foydalanuvchining til kodi (IETF)

        public string PasswordHash { get; set; }  // Foydalanuvchi parolining hash qiymati
        public string Salt { get; set; }  // Parolni himoyalash uchun ishlatiladigan tuz
        public string RefreshToken { get; set; }  // Foydalanuvchining yangilanish tokeni
        public DateTime RefreshTokenExpireDate { get; set; }  // Yangilanish tokenining amal qilish muddati
        public string ConfirmationCode { get; set; }  // Foydalanuvchi ro'yxatdan o'tishni tasdiqlash kodi

        public ICollection<ChatUser> Chats { get; set; }  // Foydalanuvchi chatlari
    }

    public class UserProfilePhotos
    {
        public int TotalCount { get; set; }                 // Maqsadli foydalanuvchining umumiy profil rasmlari soni
        public List<List<PhotoSize>> Photos { get; set; }   // So'ralgan profil rasmlari (har biri uchun 4 gacha o'lchamda)
    }
}
