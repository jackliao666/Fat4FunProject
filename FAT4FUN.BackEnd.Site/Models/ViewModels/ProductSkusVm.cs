using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class ProductSkusVm
    {
        
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductSkuName { get; set; }
        public int Price { get; set; }
        public int Sale { get; set; }
        public List<SkuItemVm> SkuItems { get; set; }

    }
}