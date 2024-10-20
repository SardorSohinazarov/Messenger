using FluentValidation;
using Messenger.Application.DataTransferObjects.Chats;

namespace Messenger.Application.Validators.Chats
{
    public class GroupCreationDtoValidator : AbstractValidator<GroupCreationDto>
    {
        public GroupCreationDtoValidator()
        {
        }
    }
}
