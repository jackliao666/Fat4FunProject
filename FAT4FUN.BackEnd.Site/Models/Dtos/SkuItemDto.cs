using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Dtos
{
    public class SkuItemDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int? SkuPrice { get; set; }
    }
}