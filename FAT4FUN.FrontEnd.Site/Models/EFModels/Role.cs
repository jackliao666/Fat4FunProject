namespace FAT4FUN.FrontEnd.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Role
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column("Role")]
        public int Role1 { get; set; }

        public virtual User User { get; set; }
    }
}
