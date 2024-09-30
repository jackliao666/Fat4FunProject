namespace FAT4FUN.BackEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string No { get; set; }

        public int PaymentMethod { get; set; }

        public int TotalAmount { get; set; }

        public int ShippingMethod { get; set; }

        [Required]
        [StringLength(50)]
        public string RecipientName { get; set; }

        [Required]
        [StringLength(50)]
        public string ShippingAddress { get; set; }

        [Required]
        [StringLength(10)]
        public string Phone { get; set; }

        public int Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual User User { get; set; }
    }
}
