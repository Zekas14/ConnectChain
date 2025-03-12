using AutoMapper;
using ConnectChain.Models;
using ConnectChain.ViewModel.Authentication;

namespace ConnectChain.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            #region Register
            CreateMap<UserRegisterRequestViewModel,User>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dst => dst.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<User,UserRegisterRequestViewModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dst => dst.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
            #endregion


        }
    }
}
