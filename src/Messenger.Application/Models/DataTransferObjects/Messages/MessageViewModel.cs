﻿using Messenger.Application.Models.DataTransferObjects.Chats;
using Messenger.Application.Models.DataTransferObjects.Users;
using Messenger.Domain.Entities;

namespace Messenger.Application.Models.DataTransferObjects.Messages
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }                          // Xabar ID
        public long? FromId { get; set; }                     // Xabarni yuborgan foydalanuvchi ID
        public UserViewModel From { get; set; }                        // Xabarni yuborgan foydalanuvchi

        //public long? SenderChatId { get; set; }               // Xabar yuborilgan chat ID
        //public Chat SenderChat { get; set; }                  // Xabar yuborilgan chat

        public long ChatId { get; set; }                      // Xabar jo'natilgan chat ID
        public ChatViewModel Chat { get; set; }                        // Xabar jo'natilgan chat

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
        public UserViewModel NewChatMember { get; set; }               // Yangi chat a'zosi
        public DateTime CreatedAt { get; set; }
    }
}
