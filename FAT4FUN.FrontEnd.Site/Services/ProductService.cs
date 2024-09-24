using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Services.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Services
{
	public class ProductService
	{

		public DtoProduct GetProduct(int id)
		{
			var db = new AppDbContext();

			var data = db.Products.Where(t => t.Id == id).Select(t => new DtoProduct
			{
				Id = t.Id,
				BrandName = t.Brand.Name,
				Name = t.Name,
				Description = t.Description,
				Status = t.Status,
				Look = t.Look,
				CreateDate = t.CreateDate
			}).FirstOrDefault();

			return data;
		}

	}
}