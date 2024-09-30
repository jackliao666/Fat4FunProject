using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class ProductSkuVm
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Sale { get; set; }
        public List<SkuItemVm> SkuItems{ get; set; }
    }
}