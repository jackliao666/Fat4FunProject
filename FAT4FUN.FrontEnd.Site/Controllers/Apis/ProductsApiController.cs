using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace FAT4FUN.FrontEnd.Site.Controllers.Apis
{
	public class ProductsApiController : ApiController
	{
		private AppDbContext db = new AppDbContext();

		// GET: api/Products
		[HttpGet]
		[Route("api/products")]
		public IHttpActionResult GetProducts()
		{
			var products = db.Products
				.Include(p => p.Brands)
				.Include(p => p.ProductCategories)
				.Include(p => p.ProductSkus.Select(s => s.SkuItems))
				.Include(p => p.Images)
				.Where(p => p.Status && p.ProductCategories.CategoryName == "PC") // 只取得啟用的產品
				.Select(p => new ProductVm
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					Category = p.ProductCategories.CategoryName,
					Brand = p.Brands.Name,
					Image = p.Images.FirstOrDefault().FileName, // 假設你只需要第一張圖片
					Specs = p.ProductSkus.Select(s => new ProductSkuVm
					{
						Name = s.Name,
						Price = s.Price,
						Sale = s.Sale,   
						SkuItems = s.SkuItems.Select(i => new SkuItemVm
						{
							Key = i.key,
							Value = i.value
						}).ToList()
					}).ToList()
				}).ToList();

			return Ok(products);
		}
	}
}
