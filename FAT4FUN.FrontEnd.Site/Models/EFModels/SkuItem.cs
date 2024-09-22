namespace FAT4FUN.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SkuItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SkuItem()
        {
            Carts = new HashSet<Cart>();
        }

        public int Id { get; set; }

        public int ProductSkuId { get; set; }

        [Required]
        [StringLength(50)]
        public string key { get; set; }

        [Required]
        [StringLength(50)]
        public string value { get; set; }

        public int? Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ProductSku ProductSku { get; set; }
    }
}
