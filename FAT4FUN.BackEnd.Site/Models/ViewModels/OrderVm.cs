using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class OrderVm
    {
        public int Id { get; set; }

        [Display(Name = "會員編號")]
        public int UserId { get; set; }

        [Display(Name = "訂單編號")]
        public string No { get; set; }

        [Display(Name = "付款方式")]
        public int PaymentMethod { get; set; }

        [Display(Name = "訂單總額")]
        public int TotalAmount { get; set; }

        [Display(Name = "取貨方式")]
        public int ShippingMethod { get; set; }

        [Display(Name = "收件人姓名")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "{0}長度不得大於50個字}")]
        public string RecipientName { get; set; }

        [Display(Name = "收件地址")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "{0}長度不得大於50個字}")]
        public string ShippingAddress { get; set; }

        [Display(Name = "聯絡電話")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "{0}長度不得大於10個字}")]
        public string Phone { get; set; }

        [Display(Name = "訂單狀態")]
        public int Status { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "最近更新日")]
        public DateTime ModifyDate { get; set; }

        // 訂單明細
        public List<OrderItemVM> OrderItems { get; set; }
    }
    public class OrderItemVM
    {
        public int Id { get; set; }

        [Display(Name = "訂單編號")]
        public int OrderId { get; set; }

        [Display(Name = "產品編號")]
        public int ProductSkuId { get; set; }

        [Display(Name = "產品名稱")]
        public string ProductName { get; set; }

        [Display(Name = "規格名稱")]
        public string SkuItemName { get; set; }

        [Display(Name = "價格")]
        public int Price { get; set; }

        [Display(Name = "數量")]
        public int Qty { get; set; }

        [Display(Name = "小計")]
        public int SubTotal { get; set; }
    }
}