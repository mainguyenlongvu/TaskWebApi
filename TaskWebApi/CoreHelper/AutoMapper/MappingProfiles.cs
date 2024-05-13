using AutoMapper;
using TaskWebApi.Model;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.CoreHelper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserEntity, UserModel>();
            CreateMap<UserModel, UserEntity>();
            CreateMap<RegisterModel, UserEntity>()
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
              .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
              .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
              .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles))
              .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));


        }
    }
}
