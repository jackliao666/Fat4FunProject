using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
    public class MemberFollowListVm
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

    }
}