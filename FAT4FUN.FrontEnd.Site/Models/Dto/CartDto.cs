using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.Dto
{
    public class CartDto
    {
        public int UserId { get; set; }
        public int ProductSkuId { get; set; }
        public int? SkuItemId { get; set; } // 如果有規格選項
        public int Qty { get; set; } // 新的數量
    }
}