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
            CreateMap<ProductDto, ProductVm>();
            //dto轉換至sku
            CreateMap<ProductSkuDto, ProductSku>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();

            CreateMap<ProductSku, ProductSkuDto>().ReverseMap();

            CreateMap<Product2Vm, Product2Dto>().ReverseMap();
            CreateMap<ProductSkuDto, ProductSkuVm>().ReverseMap();
            CreateMap<SkuItemDto, SkuItemVm>().ReverseMap();

            CreateMap<Product2Dto, ProductSku>();
            CreateMap<SkuItemDto, SkuItem>();

            //訂單管理
            CreateMap<OrderVm, OrderDto>().ReverseMap();
            CreateMap<OrderItemVM, OrderItemDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderDto, EditOrderVm>().ReverseMap();
            CreateMap<EditOrderVm, EditOrderDto>();










            CreateMap<UserDto, EditProfileVm>();

            CreateMap<EditProfileVm, EditProfileDto>();

            CreateMap<UserDto, ChangePasswordDto>();

            CreateMap<ChangePasswordVm, ChangePasswordDto>();

            CreateMap<UserCheckDto, UserVm>();

            CreateMap<UserVm, User>().ReverseMap();

        }
    }
}