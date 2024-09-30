using FAT4FUN.BackEnd.Site.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class Product2Vm
    {
        [Display(Name="產品編號")]
        public int Id { get; set; }

        [Display(Name = "產品名稱")]
        public string Name { get; set; }

        [Display(Name = "規格編號")]
        public int ProductSkuId { get; set; }

        [Display(Name = "規格名稱")]
        public string ProductSkuName { get; set; }
        public string Key { get; set; }

        [Display(Name = "升級規格名稱")]
        public string Value { get; set; }

        [Display(Name = "價格")]
        public int Price { get; set; }
        public int SkuItemId { get; set; }  
        public ProductSkuVm ProductSku { get; set; }
        public List<ProductSkuVm> ProductSkus { get; set; }
        public List<SkuItemVm> SkuItems { get; set; }      
    }
}