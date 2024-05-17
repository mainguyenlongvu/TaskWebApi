using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskWebApi.Data.Entities;
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
              .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));
            CreateMap<ApplicationEntity, ApplicationModel>().ReverseMap();
            CreateMap<ApplicationEntity, ApplicationModel>();
            CreateMap<AttachmentEntity, AttachmentModel>().ReverseMap();
            CreateMap<AttachmentEntity, AttachmentModel>();
            CreateMap<ActionResult<ApplicationModel>, ApplicationModel>();
            CreateMap<WageEntity, WageModel>();
            CreateMap<WageEntity, WageModel>().ReverseMap();

        }
    }
}
