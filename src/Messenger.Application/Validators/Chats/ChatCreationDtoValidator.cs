using FluentValidation;
using Messenger.Application.DataTransferObjects.Chats;

namespace Messenger.Application.Validators.Chats
{
    public class ChatCreationDtoValidator : AbstractValidator<ChatCreationDto>
    {
        public ChatCreationDtoValidator()
        {
            RuleFor(chat => chat.Type)
                .IsInEnum().WithMessage("Chat turi 'private', 'group', 'supergroup' yoki 'channel' bo'lishi kerak.");

            RuleFor(chat => chat.Title)
                .MaximumLength(100).When(x => x.Title is not null).WithMessage("Sarlavha 100 ta belgidan oshmasligi kerak."); // Maksimal uzunlik
        }
    }
}
