using AutoMapper;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models
{
    public class MappingProfile:Profile
    {
      public MappingProfile() 
        {
           CreateMap<User, UserDto>().ReverseMap();

            CreateMap<LoginVm, LoginDto>();

            CreateMap<UserDto, EditProfileVm>();

            CreateMap<EditProfileVm, EditProfileDto>();

            CreateMap<UserDto, ChangePasswordDto>();

            CreateMap<ChangePasswordVm, ChangePasswordDto>();

            CreateMap<UserCheckDto, UserVm>();

            CreateMap<UserVm, User>().ReverseMap();

        }
    }
}