using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
    public class CheckoutVm
    {
        [Display(Name = "收件人姓名")]
        [Required]
        [StringLength(30)]
        public string RecipientName { get; set; }

        [Display(Name = "電話")]
        [Required]
        [StringLength(10)]
        public string Phone { get; set; }

        [Display(Name = "收件人地址")]
        [Required]
        [StringLength(200)]
        public string ShippingAddress { get; set; }

        [Display(Name = "運送方法")]
        public int DeliveryMethod { get; set; }

        [Display(Name = "付款方式")]
        public int PaymentMethod { get; set; }

        [Display(Name = "發票選項")]
        public int InvoiceOption { get; set; }

        //public List<CartVm> Cart { get; set; }

    }
}