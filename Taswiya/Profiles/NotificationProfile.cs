using AutoMapper;
using ConnectChain.Models;
using ConnectChain.ViewModel.Notification.GetNotification;

namespace ConnectChain.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile() 
        {
            CreateMap< GetNotificationResponseViewModel, Notification>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
