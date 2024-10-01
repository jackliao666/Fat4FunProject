using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class EditOrderDto
    {
        public int Id { get; set; }

        [Display(Name = "訂單狀態")]
        public int Status { get; set; }

        [Display(Name = "收件人姓名")]
        public string RecipientName { get; set; }

        [Display(Name = "收件地址")]
        public string ShippingAddress { get; set; }

        [Display(Name = "聯絡電話")]
        public string Phone { get; set; }

        [Display(Name = "付款方式")]
        public int PaymentMethod { get; set; }

        [Display(Name = "取貨方式")]
        public int ShippingMethod { get; set; } 

        [Display(Name = "最近更新日")]
        public DateTime ModifyDate { get; set; }
    }
}