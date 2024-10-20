using Messenger.Domain.Entities;
using Messenger.Domain.Enums;

namespace Messenger.Application.DataTransferObjects.Chats
{
    public class ChannelCreationDto
    {
        public string Title { get; set; }  // Ixtiyoriy. Kanallar va guruh chatlari uchun sarlavha
        public string? UserName { get; set; }  // Ixtiyoriy. Kanallar uchun foydalanuvchi nomi (agar mavjud bo'lsa)

        //public ChatPhoto Photo { get; set; }  // Chat rasmi
    }
}
