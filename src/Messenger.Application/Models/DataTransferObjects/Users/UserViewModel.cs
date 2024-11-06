namespace Messenger.Application.Models.DataTransferObjects.Users
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }  // Foydalanuvchi ismi
        public string? LastName { get; set; }  // Ixtiyoriy. Foydalanuvchi familiyasi
        public string? UserName { get; set; }  // Ixtiyoriy. Foydalanuvchi foydalanuvchi nomi
    }
}
