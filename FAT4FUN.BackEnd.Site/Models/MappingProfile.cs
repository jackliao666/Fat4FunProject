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
            //vm轉換至dto 雙向
            CreateMap<ProductVm, ProductDto>().ReverseMap();
            //dto轉換至sku
            CreateMap<ProductSkuDto, ProductSku>().ReverseMap();
            CreateMap<ProductDto, Product>();



         
            


        }
    }
}