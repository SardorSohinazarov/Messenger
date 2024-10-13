using Messenger.Domain.Entities;

namespace Messenger.Application.DataTransferObjects.Chats
{
    public class ChatInviteLinkViewModel
    {
        public long Id { get; set; } // Chat havolasi identifikatori
        public string InviteLink { get; set; } // Taklif havolasi. Agar havola boshqa chat administratori tomonidan yaratilgan bo'lsa, havolaning ikkinchi qismi "..." bilan almashtiriladi
        public DateTime ExpireDate { get; set; } // Havola amal qilish muddati (Unix vaqtida)
    }
}
