namespace Messenger.Application.Models.DataTransferObjects.Messages
{
    public class MessageModificationDto
    {
        public Guid Id { get; set; }                          // Xabar ID

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
    }
}
