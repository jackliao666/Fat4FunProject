namespace FAT4FUN.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RolesString")]
    public partial class RolesString
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Roles { get; set; }
    }
}
