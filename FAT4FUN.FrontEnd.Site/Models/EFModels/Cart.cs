namespace FAT4FUN.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cart
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int ProductSkuId { get; set; }

        public int? SkuItemId { get; set; }

        public int Qty { get; set; }

        public virtual ProductSku ProductSku { get; set; }

        public virtual SkuItem SkuItem { get; set; }

        public virtual User User { get; set; }
    }
}
