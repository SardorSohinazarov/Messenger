namespace Messenger.Application.Models.DataTransferObjects.Auth.UserProfiles
{
    public class UserProfile
    {
        public long Id { get; set; }
        public string FirstName { get; set; }  // Foydalanuvchi ismi
        public string? LastName { get; set; }  // Ixtiyoriy. Foydalanuvchi familiyasi
        public string? UserName { get; set; }  // Ixtiyoriy. Foydalanuvchi foydalanuvchi nomi
        public string PhoneNumber { get; set; }  // Foydalanuvchi telefon raqami
        public string Email { get; set; }  // Ixtiyoriy. Foydalanuvchi elektron pochta manzili
        public string? LanguageCode { get; set; }  // Ixtiyoriy. Foydalanuvchining til kodi (IETF)
        public DateTime CreatedAt { get; set; }
    }
}
