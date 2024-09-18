using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
	public class ProductVm
	{
        public int Id { get; set; }
		public int BrandId { get; set; }
		public int ProductCategoryId { get; set; }
		public int? ProductSkuId { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int? Look { get; set; }
       
    }
}