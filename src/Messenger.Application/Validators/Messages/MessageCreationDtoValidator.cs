using FluentValidation;
using Messenger.Application.DataTransferObjects.Messages;

namespace Messenger.Application.Validators.Messages
{
    public class MessageCreationValidator : AbstractValidator<MessageCreationDto>
    {
        public MessageCreationValidator()
        {
            //// SenderChatId uchun validatsiya
            //RuleFor(message => message.SenderChatId)
            //    .GreaterThan(0).When(message => message.SenderChatId.HasValue) // Agar qiymat mavjud bo'lsa, 0 dan katta bo'lishi kerak.
            //    .WithMessage("Xabar yuborilgan chat ID 0 dan katta bo'lishi kerak.");

            // ChatId uchun validatsiya
            RuleFor(message => message.ChatId)
                .GreaterThan(0).WithMessage("Chat ID 0 dan katta bo'lishi kerak.");

            // Text uchun validatsiya (null bo'lishi mumkin)
            RuleFor(message => message.Text)
                .MaximumLength(500).When(message => !string.IsNullOrEmpty(message.Text))
                .WithMessage("Xabar matni 500 belgidan oshmasligi kerak."); // Faqat matn mavjud bo'lsa tekshiriladi.
        }
    }
}
