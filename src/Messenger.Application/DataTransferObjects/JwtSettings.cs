namespace Messenger.Application.DataTransferObjects
{
    public class JwtSettings
    {
        public string Key { get; set; }  // JWT maxfiy kaliti
        public string Issuer { get; set; }  // Tokenni yaratuvchi
        public string Audience { get; set; }  // Token foydalanishi uchun mo'ljallangan
        public int TokenExpirationInMinutes { get; set; }  // Token amal qilish vaqti
        public int RefreshTokenExpirationInDays { get; set; }  // Refresh token amal qilish vaqti
    }
}
