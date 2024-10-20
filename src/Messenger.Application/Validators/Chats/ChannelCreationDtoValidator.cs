using FluentValidation;
using Messenger.Application.DataTransferObjects.Chats;

namespace Messenger.Application.Validators.Chats
{
    public class ChannelCreationDtoValidator : AbstractValidator<ChannelCreationDto>
    {
        public ChannelCreationDtoValidator()
        {
        }
    }
}
