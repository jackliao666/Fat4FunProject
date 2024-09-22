namespace FAT4FUN.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        public string Path { get; set; }

        public int Sort { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
