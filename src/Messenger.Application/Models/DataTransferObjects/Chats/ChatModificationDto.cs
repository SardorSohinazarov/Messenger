namespace Messenger.Application.Models.DataTransferObjects.Chats
{
    public class ChatModificationDto
    {
        public long Id { get; set; }  // Ixtiyoriy. Chat identifikatori
        public string Title { get; set; }  // Ixtiyoriy. Supergroup'lar, kanallar va guruh chatlari uchun sarlavha
    }
}
