using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
	public class ProductCategoryVm
	{
		public int Id { get; set; }
		public string CategoryName { get; set; }
		public int DisplayOrder { get; set; }
		public bool Status { get; set; }
	}
}