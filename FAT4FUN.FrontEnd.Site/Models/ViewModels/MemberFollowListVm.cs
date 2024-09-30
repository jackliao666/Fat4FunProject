using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
    public class MemberFollowListVm
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int ProductSkuId { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public DateTime CreateDate { get; set; }

    }
}