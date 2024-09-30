using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string No { get; set; }
        public int PaymentMethod { get; set; }
        public int TotalAmount { get; set; }
        public int ShippingMethod { get; set; }
        public string RecipientName { get; set; }
        public string ShippingAddress { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        // 訂單明細
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductSkuId { get; set; }

         public string ProductName { get; set; }

        public string SkuItemName { get; set; }

        public int Price { get; set; }

        public int Qty { get; set; }

        public int SubTotal { get; set; }
    }
}