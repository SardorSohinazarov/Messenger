using FluentValidation;
using Messenger.Application.Models.DataTransferObjects.Messages;

namespace Messenger.Application.Validators.Messages
{
    public class MessageModificationDtoValidator : AbstractValidator<MessageModificationDto>
    {
        public MessageModificationDtoValidator()
        {
            // Id uchun validatsiya
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Xabar ID bo'sh bo'lmasligi kerak.")
                .NotEqual(Guid.Empty).WithMessage("Xabar ID bo'sh qiymatga teng bo'lmasligi kerak.");

            // Text uchun validatsiya (null bo'lishi mumkin)
            RuleFor(message => message.Text)
                .MaximumLength(500).When(message => !string.IsNullOrEmpty(message.Text))
                .WithMessage("Xabar matni 500 belgidan oshmasligi kerak."); // Faqat matn mavjud bo'lsa tekshiriladi.
        }
    }
}
