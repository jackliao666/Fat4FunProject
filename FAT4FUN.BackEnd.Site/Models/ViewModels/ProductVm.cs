using FAT4FUN.BackEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class ProductVm
    {
        [Display(Name ="產品編號")]
        public int Id { get; set; }
        //[Required(ErrorMessage = "請選擇商品分類")]
        public int CategoryId { get; set; }

        [Display(Name = "產品類別")]
        //[Required(ErrorMessage ="必填]")]
        public string CategoryName { get; set; }
        //[Required(ErrorMessage = "請選擇品牌")]
        public int BrandId { get; set; }

        [Display(Name = "品牌名稱")]

        public string BrandName { get; set; }

        [Display(Name = "產品名稱")]
        //[Required(ErrorMessage = "請輸入商品名稱")]
        public string Name { get; set; } 

        [Display(Name = "產品描述")]
        //[Required(ErrorMessage = "請輸入商品描述")]
        public string Description { get; set; }

        public int ProductSkuId { get; set; }   
        [Display(Name="規格名稱")]
        public string ProductSkuName { get; set; }

        [Display(Name = "定價")]
        //[Range(0, double.MaxValue, ErrorMessage = "價格必須大於0")]
        public int Price { get; set; }
        
        [Display(Name = "特價")]
        //[Range(0, double.MaxValue, ErrorMessage = "特價必須大於0")]
        public int Sale { get; set; }

        [Display(Name ="上下架設定")]
        //[Required(ErrorMessage = "請選擇狀態")]
        public bool Status { get; set; }

        [Display(Name = "建立日期")]
        public DateTime? CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "修改日期")]
        public DateTime? ModifyDate { get; set; } = DateTime.Now;
        public ProductSkuVm ProductSku { get; set; }
        public List<ProductSkusVm> ProductSkus { get; set; }


    }
}