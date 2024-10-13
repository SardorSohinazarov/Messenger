using FluentValidation;
using Messenger.Application.DataTransferObjects.Chats;

namespace Messenger.Application.Validators.Chats
{
    public class ChatModificationDtoValidator : AbstractValidator<ChatModificationDto>
    {
        public ChatModificationDtoValidator()
        {
            RuleFor(chat => chat.Id)
                .GreaterThan(0).WithMessage("Chat identifikatori 0 dan katta bo'lishi kerak.");

            RuleFor(chat => chat.Title)
                .NotEmpty().WithMessage("Sarlavha bo'sh bo'lmasligi kerak.")
                .MaximumLength(100).WithMessage("Sarlavha 100 ta belgidan oshmasligi kerak.");
        }
    }
}
