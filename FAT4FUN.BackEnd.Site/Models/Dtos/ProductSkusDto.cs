using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class ProductSkusDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Sale { get; set; }
        public List<SkuItemDto> SkuItems { get; set; }
        public ProductDto  Product { get; set; }
    }
}