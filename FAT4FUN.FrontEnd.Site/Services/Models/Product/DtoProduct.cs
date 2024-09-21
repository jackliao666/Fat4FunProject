using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Services.Models.Product
{
	public class DtoProduct
	{
        public int Id { get; set; }
        public string BrandName { get; set; }
        public int ProductCategoryId{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int Look { get; set; }
        public DateTime CreateDate { get; set; }


    }
}