using AutoMapper;
using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Domain.Entities;

namespace Messenger.Application.Common
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ChatCreationDto, Chat>();
            CreateMap<Chat, ChatViewModel>();
            CreateMap<ChatModificationDto, Chat>();
            CreateMap<ChatUser, ChatUserViewModel>();
            CreateMap<ChatInviteLink, ChatInviteLinkViewModel>();

            CreateMap<Chat, ChatDetailsViewModel>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
                .ForMember(dest => dest.InviteLinks, opt => opt.MapFrom(src => src.InviteLinks));
        }
    }
}
