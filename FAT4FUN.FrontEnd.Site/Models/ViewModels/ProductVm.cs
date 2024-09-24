using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
	public class ProductVm
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }
		public string CategoryName { get; set; }
        public string Brand { get; set; }
		public string Image { get; set; }
		public List<ProdctImageVm> ImageList { get; set; }
        public int Look { get; set; }
        public List<ProductSkuVm> Specs { get; set; }
	}
}