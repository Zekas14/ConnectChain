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
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<User,UserRegisterRequestViewModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
            #endregion


        }
    }
}
