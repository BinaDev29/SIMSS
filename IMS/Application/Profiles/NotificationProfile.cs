// Application/Profiles/NotificationProfile.cs
using Application.DTOs.Notification;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            
            CreateMap<CreateNotificationDto, Notification>();
        }
    }
}