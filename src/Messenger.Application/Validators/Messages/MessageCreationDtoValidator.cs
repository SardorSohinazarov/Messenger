using FluentValidation;
using Messenger.Application.DataTransferObjects.Messages;

namespace Messenger.Application.Validators.Messages
{
    public class MessageCreationValidator : AbstractValidator<MessageCreationDto>
    {
        public MessageCreationValidator()
        {
            // FromId uchun validatsiya
            RuleFor(message => message.FromId)
                .GreaterThan(0).When(message => message.FromId.HasValue) // Agar qiymat mavjud bo'lsa, 0 dan katta bo'lishi kerak.
                .WithMessage("Foydalanuvchi ID 0 dan katta bo'lishi kerak.");

            // SenderChatId uchun validatsiya
            RuleFor(message => message.SenderChatId)
                .GreaterThan(0).When(message => message.SenderChatId.HasValue) // Agar qiymat mavjud bo'lsa, 0 dan katta bo'lishi kerak.
                .WithMessage("Xabar yuborilgan chat ID 0 dan katta bo'lishi kerak.");

            // ChatId uchun validatsiya
            RuleFor(message => message.ChatId)
                .GreaterThan(0).WithMessage("Chat ID 0 dan katta bo'lishi kerak.");

            // Text uchun validatsiya (null bo'lishi mumkin)
            RuleFor(message => message.Text)
                .MaximumLength(500).When(message => !string.IsNullOrEmpty(message.Text))
                .WithMessage("Xabar matni 500 belgidan oshmasligi kerak."); // Faqat matn mavjud bo'lsa tekshiriladi.

            // NewChatTitle uchun validatsiya (null bo'lishi mumkin)
            RuleFor(message => message.NewChatTitle)
                .MaximumLength(100).When(message => !string.IsNullOrEmpty(message.NewChatTitle))
                .WithMessage("Chatning yangi sarlavhasi 100 belgidan oshmasligi kerak."); // Faqat sarlavha mavjud bo'lsa tekshiriladi.

            // NewChatMemberId uchun validatsiya (null bo'lishi mumkin)
            RuleFor(message => message.NewChatMemberId)
                .GreaterThan(0).When(message => message.NewChatMemberId.HasValue)
                .WithMessage("New chat member ID 0 dan katta bo'lishi kerak."); // Faqat ID mavjud bo'lsa tekshiriladi.
        }
    }
}
