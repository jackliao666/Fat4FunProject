using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class Product2Dto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductSkuId { get; set; }
        public string ProductSkuName { get; set; }   
        public string Key { get; set; }
        public string Value { get; set; }
        public int Price { get; set; }
        public int SkuItemId { get; set; }
        public ProductSkuDto ProductSku { get; set; }
        public List<ProductSkuDto> ProductSkus { get; set; }
        public List<SkuItemDto> SkuItems { get; set; }
    }
}