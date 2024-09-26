using FAT4FUN.FrontEnd.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
    public class CartVm
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public int ProductSkuId { get; set; }
        public ProductSkuVm Product { get; set; }

        public int? SkuItemId { get; set; }

        public SkuItemVm SkuItem { get; set; }

        public int Qty { get; set; }


        public int SubTotal
        {
            get
            {

                int productPrice = Product?.Price ?? 0; // 確保不為 null
                int skuPrice = SkuItem?.SkuPrice ?? 0; // 確保不為 null

                return (productPrice * Qty) + (skuPrice * Qty);
            }
        }
        
        
    }
}