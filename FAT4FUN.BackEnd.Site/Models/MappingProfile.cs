using AutoMapper;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FAT4FUN.BackEnd.Site.Models
{
    public class MappingProfile:Profile
    {
       public MappingProfile() 
        {
            CreateMap<LoginVm, LoginDto>();
        }
    }
}