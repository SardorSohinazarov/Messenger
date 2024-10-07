namespace Messenger.Domain.Entities
{
    public class Update
    {
        public Message Message { get; set; }  // (Ixtiyoriy) Har qanday turdagi yangi kelayotgan xabar - matn, foto, sticker va boshqalar.
        public Message EditedMessage { get; set; }  // (Ixtiyoriy) Botga ma'lum bo'lgan va tahrir qilingan xabarning yangi versiyasi. Ushbu yangilanish ba'zan botingiz tomonidan faol ishlatilmaydigan xabar maydonlaridagi o'zgarishlar tomonidan ishga tushirilishi mumkin.
        public Message ChannelPost { get; set; }  // (Ixtiyoriy) Har qanday turdagi yangi kelayotgan kanal posti - matn, foto, sticker va boshqalar.
        public Message EditedChannelPost { get; set; }  // (Ixtiyoriy) Botga ma'lum bo'lgan va tahrir qilingan kanal postining yangi versiyasi. Ushbu yangilanish ba'zan botingiz tomonidan faol ishlatilmaydigan xabar maydonlaridagi o'zgarishlar tomonidan ishga tushirilishi mumkin.
    }
}
