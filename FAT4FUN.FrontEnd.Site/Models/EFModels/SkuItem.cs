namespace FAT4FUN.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SkuItem
    {
        public int Id { get; set; }

        public int ProductSkuId { get; set; }

        [Required]
        [StringLength(50)]
        public string key { get; set; }

        [Required]
        [StringLength(50)]
        public string value { get; set; }

        public virtual ProductSku ProductSku { get; set; }
    }
}
