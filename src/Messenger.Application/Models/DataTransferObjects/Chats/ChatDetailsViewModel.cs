﻿using Messenger.Application.Models.DataTransferObjects.ChatUsers;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;

namespace Messenger.Application.Models.DataTransferObjects.Chats
{
    public class ChatDetailsViewModel
    {
        public long Id { get; set; }
        public EChatType Type { get; set; }  // Chat turi, “private”, “group”, “supergroup” yoki “channel” bo'lishi mumkin
        public string Title { get; set; }  // Ixtiyoriy. Supergroup'lar, kanallar va guruh chatlari uchun sarlavha
        public string Username { get; set; }  // Ixtiyoriy. Shaxsiy chatlar, supergroup'lar va kanallar uchun foydalanuvchi nomi (agar mavjud bo'lsa)
        public string FirstName { get; set; }  // Ixtiyoriy. Shaxsiy chatdagi boshqa tomonning ismi
        public string LastName { get; set; }  // Ixtiyoriy. Shaxsiy chatdagi boshqa tomonning familiyasi

        public ICollection<ChatUserViewModel> Users { get; set; }  // Chat foydalanuvchilari
        public ICollection<ChatInviteLinkViewModel> InviteLinks { get; set; }  // Chat uchun taklif havolalari
        public ChatPhoto Photo { get; set; }  // Chat rasmi
    }
}
