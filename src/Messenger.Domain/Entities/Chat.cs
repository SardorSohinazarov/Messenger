using Messenger.Domain.Common;
using Messenger.Domain.Enums;

namespace Messenger.Domain.Entities
{
    public class Chat : BaseEntity<long>, IAuditable, ISoftDeletable
    {
        public EChatType Type { get; set; }  // Chat turi, “private”, “group”, “supergroup” yoki “channel” bo'lishi mumkin
        public string? Title { get; set; }  // Ixtiyoriy. Supergroup'lar, kanallar va guruh chatlari uchun sarlavha
        public string? UserName { get; set; }  // Ixtiyoriy. Shaxsiy chatlar, supergroup'lar va kanallar uchun foydalanuvchi nomi (agar mavjud bo'lsa)
        public string? FirstName { get; set; }  // Ixtiyoriy. Shaxsiy chatdagi boshqa tomonning ismi
        public string? LastName { get; set; }  // Ixtiyoriy. Shaxsiy chatdagi boshqa tomonning familiyasi

        public ICollection<ChatUser> Users { get; set; }  // Chat foydalanuvchilari
        public ICollection<ChatInviteLink> InviteLinks { get; set; }  // Chat uchun taklif havolalari
        public ICollection<Message> Messages { get; set; } // Chatga jo'natilgan messagelar
        public ChatPhoto Photo { get; set; }  // Chat rasmi

        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public class ChatFullInfo : BaseEntity<long>, IAuditable, ISoftDeletable
    {
        public EChatType Type { get; set; } // Chat turi: "private", "group", "supergroup" yoki "channel".
        public string Title { get; set; } // Supergroup, kanallar va guruh chatlar uchun sarlavha.
        public string Username { get; set; } // Maxsus chatlar, supergroup va kanallar uchun username.
        public string FirstName { get; set; } // Maxsus chatdagi boshqa tomonning ismi.
        public string LastName { get; set; } // Maxsus chatdagi boshqa tomonning familiyasi.
        public ChatPhoto Photo { get; set; } // Chat rasmi.
        public Chat PersonalChat { get; set; } // Maxsus chatdagi foydalanuvchining shaxsiy kanali.
        public string Bio { get; set; } // Maxsus chatdagi boshqa tomonning bio ma'lumotlari.
        public string Description { get; set; } // Guruh, supergroup va kanal chatlari uchun tavsif.
        public string InviteLink { get; set; } // Guruh, supergroup va kanal chatlari uchun asosiy taklif havolasi.
        public long? LinkedChatId { get; set; } // Bog'langan chat uchun unikal identifikator.

        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public class ChatPhoto : BaseEntity<long>, IAuditable, ISoftDeletable
    {
        public string SmallFileId { get; set; }  // Kichik (160x160) chat fotosining fayl identifikatori. Bu file_id faqat foto yuklash uchun ishlatilishi mumkin va foto o'zgartirilmaguncha amal qiladi.
        public string SmallFileUniqueId { get; set; }  // Kichik (160x160) chat fotosining noyob fayl identifikatori, vaqt o‘tishi bilan va turli botlar uchun bir xil bo‘lishi kerak. Faylni yuklab olish yoki qayta ishlatish uchun ishlatilmaydi.
        public string BigFileId { get; set; }  // Katta (640x640) chat fotosining fayl identifikatori. Bu file_id faqat foto yuklash uchun ishlatilishi mumkin va foto o'zgartirilmaguncha amal qiladi.
        public string BigFileUniqueId { get; set; }  // Katta (640x640) chat fotosining noyob fayl identifikatori, vaqt o‘tishi bilan va turli botlar uchun bir xil bo‘lishi kerak. Faylni yuklab olish yoki qayta ishlatish uchun ishlatilmaydi.

        public long ChatId { get; set; }  // Chat ID
        public Chat Chat { get; set; }  // Chat entity bilan bog'lanish

        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public class ChatInviteLink : BaseEntity<long>, IAuditable, ISoftDeletable
    {
        public string InviteLink { get; set; } // Taklif havolasi. Agar havola boshqa chat administratori tomonidan yaratilgan bo'lsa, havolaning ikkinchi qismi "..." bilan almashtiriladi
        public DateTime ExpireDate { get; set; } // Havola amal qilish muddati (Unix vaqtida)

        public long ChatId { get; set; }  // Chat ID
        public Chat Chat { get; set; }  // Chat entity bilan bog'lanish

        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
