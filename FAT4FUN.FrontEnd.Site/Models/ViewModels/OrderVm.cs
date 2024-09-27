using FAT4FUN.FrontEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
    public class OrderVm
    {
        public int Id { get; set; }
        public string No { get; set; }
        public int PaymentMethod { get; set; }
        public int TotalAmount { get; set; }
        //public int ShippingMethod { get; set; }
        //public string RecipientName { get; set; }
        //public string ShippingAddress { get; set; }
        //public string Phone { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        // 訂單明細
        public List<OrderItemVM> OrderItems { get; set; }
    }
    public class OrderItemVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductSkuId { get; set; }
        public string ProductName { get; set; }
        //public int? SkuItemId { get; set; }
        public string SkuItemName { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public int SubTotal { get; set; }
    }
}