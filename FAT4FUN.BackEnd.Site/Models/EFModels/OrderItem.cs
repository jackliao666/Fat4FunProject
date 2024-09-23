namespace FAT4FUN.BackEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductSkuId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        public int? SkuItemId { get; set; }

        [StringLength(50)]
        public string SkuItemName { get; set; }

        public int Price { get; set; }

        public int Qty { get; set; }

        public int SubTotal { get; set; }

        public virtual ProductSku ProductSku { get; set; }

        public virtual Order Order { get; set; }

        public virtual SkuItem SkuItem { get; set; }
    }
}
