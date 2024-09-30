using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.ViewModels
{
    public class SkuItemVm
    {
        public int Id { get; set; }
        public int ProductSkuId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int? Price { get; set; }
    }
}