using Messenger.Domain.Common;

namespace Messenger.Domain.Entities
{
    public class Message : Auditable<Guid>
    {
        public long? FromId { get; set; }                     // Xabarni yuborgan foydalanuvchi ID
        public User From { get; set; }                        // Xabarni yuborgan foydalanuvchi

        public long? SenderChatId { get; set; }               // Xabar yuborilgan chat ID
        public Chat SenderChat { get; set; }                  // Xabar yuborilgan chat

        public long ChatId { get; set; }                      // Xabar jo'natilgan chat ID
        public Chat Chat { get; set; }                        // Xabar jo'natilgan chat

        public string Text { get; set; }                      // Xabar matni
        //public Animation Animation { get; set; }            // Xabar bilan birga kelgan animatsiya
        //public Audio Audio { get; set; }                    // Xabarga biriktirilgan audio
        //public Document Document { get; set; }              // Xabarga biriktirilgan hujjat
        //public List<PhotoSize> Photo { get; set; }          // Xabarga biriktirilgan foto ro'yxati
        //public Video Video { get; set; }                    // Xabarga biriktirilgan video
        //public Voice Voice { get; set; }                    // Xabarga biriktirilgan ovozli xabar
        //public Contact Contact { get; set; }                // Xabarga biriktirilgan kontakt
        //public Location Location { get; set; }              // Xabarga biriktirilgan joylashuv
        //public Story Story { get; set; }                    // Xabarga biriktirilgan hikoya
        //public string Caption { get; set; }                 // Xabar bilan birga kelgan media fayl sarlavhasi

        public string NewChatTitle { get; set; }              // Chatning yangi sarlavhasi
        //public List<PhotoSize> NewChatPhoto { get; set; }   // Chatning yangi fotosi
        public bool? DeleteChatPhoto { get; set; }            // Chat fotosi o'chirilgan bo'lsa True
        public bool? GroupChatCreated { get; set; }           // Guruh chat yaratilgan bo'lsa True
        public bool? ChannelChatCreated { get; set; }         // Kanal chat yaratilgan bo'lsa True
        public string NewChatMemberId { get; set; }           // Yangi chat a'zosi ID

        public User NewChatMember { get; set; }               // Yangi chat a'zosi
    }

    public class PhotoSize : Auditable<Guid>
    {
        public string FileId { get; set; }  // Fayl uchun identifikator, uni yuklab olish yoki qayta ishlatish uchun ishlatilishi mumkin
        public string FileUniqueId { get; set; }  // Fayl uchun noyob identifikator, vaqt o‘tishi bilan va turli botlar uchun bir xil bo‘lishi kerak. Faylni yuklab olish yoki qayta ishlatish uchun ishlatilmaydi.
        public int Width { get; set; }  // Fotosuratning kengligi
        public int Height { get; set; }  // Fotosuratning balandligi
        public int? FileSize { get; set; }  // (Ixtiyoriy) Fayl o'lchami baytlarda
    }

    public class Animation : Auditable<Guid>
    {
        public string FileId { get; set; }  // Fayl uchun identifikator, uni yuklab olish yoki qayta ishlatish uchun ishlatilishi mumkin
        public string FileUniqueId { get; set; }  // Fayl uchun noyob identifikator, vaqt o‘tishi bilan va turli botlar uchun bir xil bo‘lishi kerak. Faylni yuklab olish yoki qayta ishlatish uchun ishlatilmaydi.
        public int Width { get; set; }  // Videoning kengligi, jo'natuvchi tomonidan belgilangan
        public int Height { get; set; }  // Videoning balandligi, jo'natuvchi tomonidan belgilangan
        public int Duration { get; set; }  // Videoning davomiyligi, soniyalarda, jo'natuvchi tomonidan belgilangan
        public PhotoSize Thumbnail { get; set; }  // Animatsiya mini rasm, jo'natuvchi tomonidan belgilangan
        public string FileName { get; set; }  // Animatsiyaning asl fayl nomi, jo'natuvchi tomonidan belgilangan
        public string MimeType { get; set; }  // Faylning MIME turi, jo'natuvchi tomonidan belgilangan
        public int? FileSize { get; set; }  // (Ixtiyoriy) Fayl o'lchami baytlarda. 2^31 dan katta bo'lishi mumkin va ba'zi dasturlash tillari uni talqin qilishda qiyinchiliklarga duch kelishi mumkin.
    }

    public class Audio : Auditable<Guid>
    {
        public string FileId { get; set; }  // Fayl uchun identifikator, uni yuklab olish yoki qayta ishlatish uchun ishlatilishi mumkin
        public string FileUniqueId { get; set; }  // Fayl uchun noyob identifikator, vaqt o‘tishi bilan va turli botlar uchun bir xil bo‘lishi kerak. Faylni yuklab olish yoki qayta ishlatish uchun ishlatilmaydi.
        public int Duration { get; set; }  // Audio davomiyligi, soniyalarda, jo'natuvchi tomonidan belgilangan
        public string Performer { get; set; }  // (Ixtiyoriy) Audio ijrochisi, jo'natuvchi yoki audio teglari tomonidan belgilangan
        public string Title { get; set; }  // (Ixtiyoriy) Audio sarlavhasi, jo'natuvchi yoki audio teglari tomonidan belgilangan
        public string FileName { get; set; }  // (Ixtiyoriy) Asl fayl nomi, jo'natuvchi tomonidan belgilangan
        public string MimeType { get; set; }  // (Ixtiyoriy) Faylning MIME turi, jo'natuvchi tomonidan belgilangan
        public int? FileSize { get; set; }  // (Ixtiyoriy) Fayl o'lchami baytlarda. 2^31 dan katta bo'lishi mumkin va ba'zi dasturlash tillari uni talqin qilishda qiyinchiliklarga duch kelishi mumkin.
        public PhotoSize Thumbnail { get; set; }  // (Ixtiyoriy) Muzik faylning albom qopqog'ining mini rasmi
    }

    public class Document : Auditable<Guid>
    {
        public string FileId { get; set; }  // Fayl uchun identifikator, uni yuklab olish yoki qayta ishlatish uchun ishlatilishi mumkin
        public string FileUniqueId { get; set; }  // Fayl uchun noyob identifikator, vaqt o‘tishi bilan va turli botlar uchun bir xil bo‘lishi kerak. Faylni yuklab olish yoki qayta ishlatish uchun ishlatilmaydi.
        public PhotoSize Thumbnail { get; set; }  // (Ixtiyoriy) Hujjat mini rasm, jo'natuvchi tomonidan belgilangan
        public string FileName { get; set; }  // (Ixtiyoriy) Asl fayl nomi, jo'natuvchi tomonidan belgilangan
        public string MimeType { get; set; }  // (Ixtiyoriy) Faylning MIME turi, jo'natuvchi tomonidan belgilangan
        public int? FileSize { get; set; }  // (Ixtiyoriy) Fayl o'lchami baytlarda. 2^31 dan katta bo'lishi mumkin va ba'zi dasturlash tillari uni talqin qilishda qiyinchiliklarga duch kelishi mumkin.
    }

    public class Story : Auditable<Guid>
    {
        public Chat Chat { get; set; }  // Hikoyani joylashtirgan chat
        public int Id { get; set; }  // Chatdagi hikoyaning noyub identifikatori
    }

    public class Video : Auditable<Guid>
    {
        public string FileId { get; set; }  // Fayl uchun identifikator, uni yuklab olish yoki qayta ishlatish uchun ishlatilishi mumkin
        public string FileUniqueId { get; set; }  // Fayl uchun noyub identifikator, vaqt o‘tishi bilan va turli botlar uchun bir xil bo‘lishi kerak. Faylni yuklab olish yoki qayta ishlatish uchun ishlatilmaydi.
        public int Width { get; set; }  // Videoning kengligi, jo'natuvchi tomonidan belgilangan
        public int Height { get; set; }  // Videoning balandligi, jo'natuvchi tomonidan belgilangan
        public int Duration { get; set; }  // Videoning davomiyligi, soniyalarda, jo'natuvchi tomonidan belgilangan
        public PhotoSize Thumbnail { get; set; }  // Videoning mini rasm
        public string FileName { get; set; }  // (Ixtiyoriy) Asl fayl nomi, jo'natuvchi tomonidan belgilangan
        public string MimeType { get; set; }  // (Ixtiyoriy) Faylning MIME turi, jo'natuvchi tomonidan belgilangan
        public int? FileSize { get; set; }  // (Ixtiyoriy) Fayl o'lchami baytlarda. 2^31 dan katta bo'lishi mumkin va ba'zi dasturlash tillari uni talqin qilishda qiyinchiliklarga duch kelishi mumkin.
    }

    public class Voice : Auditable<Guid>
    {
        public string FileId { get; set; }  // Fayl uchun identifikator, uni yuklab olish yoki qayta ishlatish uchun ishlatilishi mumkin
        public string FileUniqueId { get; set; }  // Fayl uchun noyob identifikator, vaqt o‘tishi bilan va turli botlar uchun bir xil bo‘lishi kerak. Faylni yuklab olish yoki qayta ishlatish uchun ishlatilmaydi.
        public int Duration { get; set; }  // Audio davomiyligi, soniyalarda, jo'natuvchi tomonidan belgilangan
        public string MimeType { get; set; }  // (Ixtiyoriy) Faylning MIME turi, jo'natuvchi tomonidan belgilangan
        public int? FileSize { get; set; }  // (Ixtiyoriy) Fayl o'lchami baytlarda. 2^31 dan katta bo'lishi mumkin va ba'zi dasturlash tillari uni talqin qilishda qiyinchiliklarga duch kelishi mumkin. Lekin bu 52 ta muhim bitlardan ortiq bo'lmasligi kerak, shuning uchun signed 64-bit integer yoki double-precision float turi bu qiymatni saqlash uchun xavfsizdir.
    }

    public class Contact : Auditable<Guid>
    {
        public string PhoneNumber { get; set; }  // Kontaktning telefon raqami
        public string FirstName { get; set; }  // Kontaktning ism
        public string LastName { get; set; }  // (Ixtiyoriy) Kontaktning familiyasi
        public int? UserId { get; set; }  // (Ixtiyoriy) Kontaktning Telegramdagi foydalanuvchi identifikatori. Bu raqam 32 ta muhim bitlardan ko'proq bo'lishi mumkin va ba'zi dasturlash tillari uni talqin qilishda qiyinchiliklarga duch kelishi mumkin. Lekin bu 52 ta muhim bitlardan ortiq bo'lmasligi kerak, shuning uchun 64-bit integer yoki double-precision float turi bu identifikatorni saqlash uchun xavfsizdir.
        public string Vcard { get; set; }  // (Ixtiyoriy) Kontakt haqida qo'shimcha ma'lumot, vCard shaklida
    }

    public class Location : Auditable<Guid>
    {
        public float Latitude { get; set; }  // Jo'natuvchi tomonidan belgilangan kenglik
        public float Longitude { get; set; }  // Jo'natuvchi tomonidan belgilangan uzunlik
        public float? HorizontalAccuracy { get; set; }  // (Ixtiyoriy) Joylashuv uchun noaniqlik radiusi, metrda o'lchanadi; 0-1500
        public int? LivePeriod { get; set; }  // (Ixtiyoriy) Xabar yuborish sanasiga nisbatan joylashuvni yangilash vaqti; soniyalarda. Faqat faol jonli joylashuvlar uchun.
        public int? Heading { get; set; }  // (Ixtiyoriy) Foydalanuvchi harakatlanayotgan yo'nalish, darajalarda; 1-360. Faqat faol jonli joylashuvlar uchun.
        public int? ProximityAlertRadius { get; set; }  // (Ixtiyoriy) Boshqa chat a'zosiga yaqinlashish haqida ogohlantirishlar uchun maksimal masofa, metrda. Faqat jo'natilgan jonli joylashuvlar uchun.
    }
}
