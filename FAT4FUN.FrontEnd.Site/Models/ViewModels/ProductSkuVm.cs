using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
	public class ProductSkuVm
	{
		public string Name { get; set; }
		public int Price { get; set; }
		public int? Sale { get; set; }
		public List<SkuItemVm> SkuItems { get; set; }
	}
}