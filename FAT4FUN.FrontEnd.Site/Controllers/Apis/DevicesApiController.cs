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

	public class DevicesApiController : ApiController
    {
		private AppDbContext db = new AppDbContext();

		[Route("api/device/GetProducts")]
		[HttpGet]
		public IHttpActionResult GetProduct(int? maxPrice = null, int? minPrice = null, string brand = null, string categoryName = null, string key = null, string value = null)
        {
			var products = db.Products
				.Include(p => p.Brand)
				.Include(p => p.ProductCategory)
				.Include(p => p.ProductSkus.Select(s => s.SkuItems))
				.Include(p => p.Images)
				.Where(p => p.Status) // 只取得啟用的產品
				.Select(p => new ProductVm
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					CategoryName = p.ProductCategory.CategoryName,
					Brand = p.Brand.Name,
					Image = p.Images.FirstOrDefault().Path, // 假設你只需要第一張圖片
					Specs = p.ProductSkus.Select(s => new ProductSkuVm
					{
						Name = s.Name,
						Price = s.Price,
						Sale = s.Sale,
						SkuItems = s.SkuItems.Select(i => new SkuItemVm
						{
							Key = i.key,
							Value = i.value,
							SkuPrice = i.Price
						}).ToList()
					}).ToList()
				}).ToList();

			if (!string.IsNullOrEmpty(categoryName))
			{
				products = products.Where(p => p.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).ToList();
			}
			if (!string.IsNullOrEmpty(brand))
			{
				products = products.Where(p => p.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase)).ToList();
			}
			if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
			{
				products = products.Where(p => p.Specs.Any(s => s.SkuItems.Any(i => i.KeyList.Contains(key) && i.ValueList.Contains(value)))).ToList();
			}
			if (maxPrice.HasValue)
			{
				products = products.Where(p => p.Specs.Any(s => s.Price <= maxPrice.Value)).ToList();
			}
			if (minPrice.HasValue)
			{
				products = products.Where(p => p.Specs.Any(s => s.Price >= minPrice.Value)).ToList();
			}

			return Ok(products);
		}
    }
}
