namespace Messenger.Application.Models.DataTransferObjects.Chats
{
    public class GroupCreationDto
    {
        public string Title { get; set; }  // Ixtiyoriy. Guruh chatlari uchun sarlavha
        public string? UserName { get; set; }  // Ixtiyoriy. Group uchun foydalanuvchi nomi (agar mavjud bo'lsa)

        //public ChatPhoto Photo { get; set; }  // Chat rasmi
    }
}
