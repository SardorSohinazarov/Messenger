namespace Messenger.Application.Models.DataTransferObjects.Auth.Google
{
    public class GoogleUserInfo
    {
        public string Id { get; set; }          // Foydalanuvchi ID
        public string Email { get; set; }       // Foydalanuvchi emaili
        public string Name { get; set; }        // Foydalanuvchi ismi
        public string Picture { get; set; }     // Foydalanuvchi rasm URL
    }
}
