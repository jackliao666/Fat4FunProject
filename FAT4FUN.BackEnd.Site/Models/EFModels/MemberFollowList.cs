namespace FAT4FUN.BackEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemberFollowList
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
