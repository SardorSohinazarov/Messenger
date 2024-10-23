﻿using AutoMapper;
using Messenger.Application.DataTransferObjects.Auth.UserProfiles;
using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.ChatUsers;
using Messenger.Application.DataTransferObjects.Messages;
using Messenger.Application.DataTransferObjects.Users;
using Messenger.Domain.Entities;

namespace Messenger.Application.Common
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ChannelCreationDto, Chat>();
            CreateMap<GroupCreationDto, Chat>();
            CreateMap<Chat, ChatViewModel>();
            CreateMap<ChatModificationDto, Chat>();
            CreateMap<ChatUser, ChatUserViewModel>();
            CreateMap<ChatInviteLink, ChatInviteLinkViewModel>();

            CreateMap<Chat, ChatDetailsViewModel>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
                .ForMember(dest => dest.InviteLinks, opt => opt.MapFrom(src => src.InviteLinks));

            CreateMap<MessageCreationDto, Message>();
            CreateMap<MessageModificationDto, Message>();
            CreateMap<Message, MessageViewModel>();

            CreateMap<User, UserProfile>();
            CreateMap<User, UserViewModel>();
            CreateMap<UserProfileModificationDto, User>();
        }
    }
}
