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

        public int ProuctsId { get; set; }

        public int Qty { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
