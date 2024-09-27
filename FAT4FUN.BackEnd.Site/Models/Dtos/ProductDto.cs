using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class ProductDto
    {
        public ProductDto()
        {
            
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }    
        public string Name { get; set; }    
        public string Description { get; set; }
        public int ProductSkuId { get; set; }
        public string ProductSkuName { get; set; }
        public int Price { get; set; }
        public int Sale { get; set; }
        public bool Status { get; set; }      
        public DateTime CreateDate { get; set; }    
        public DateTime ModifyDate { get; set; }
        public ProductSkuDto ProductSku { get; set; }
        public List<ProductSkusDto> ProductSkus { get; set; }


    }
}   