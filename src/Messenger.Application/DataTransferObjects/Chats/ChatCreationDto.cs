using Messenger.Domain.Entities;
using Messenger.Domain.Enums;

namespace Messenger.Application.DataTransferObjects.Chats
{
    public class ChatCreationDto
    {
        public EChatType Type { get; set; }  // Chat turi, “private”, “group”, “supergroup” yoki “channel” bo'lishi mumkin
        public string Title { get; set; }  // Ixtiyoriy. Supergroup'lar, kanallar va guruh chatlari uchun sarlavha
    }
}
