using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
    public class CartVm
    {
        public int UserId { get; set; }
        public int ProductSkuId { get; set; }
        public int? SkuItemId { get; set; }
        public int Qty { get; set; }
    }
}