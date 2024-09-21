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
		[Route("api/products/GetProducts")]
		[HttpGet]
		public IHttpActionResult GetProducts(int? maxPrice = null, int? minPrice = null, string brand = null,
			string categoryName = null, string key = null, string value = null, string sort = null, string accordion = null)
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
				Image = p.Images.FirstOrDefault().FileName, // 假設你只需要第一張圖片
				Look = p.Look,
				Specs = p.ProductSkus.Select(s => new ProductSkuVm
				{
					Id = s.Id,
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
			// 排序邏輯
			if (string.IsNullOrEmpty(sort) || sort == "name")
			{
				// 默認按照名稱排序
				products = products.OrderBy(p => p.Name).ToList();
			}
			else if (sort == "price_asc")
			{
				// 按價格升序排序
				products = products.OrderBy(p => p.Specs.Min(s => s.Price)).ToList();
			}
			else if (sort == "price_desc")
			{
				// 按價格降序排序
				products = products.OrderByDescending(p => p.Specs.Max(s => s.Price)).ToList();
			}
			if (!string.IsNullOrEmpty(accordion))
			{
				if (accordion.Equals("INTEL", StringComparison.OrdinalIgnoreCase))
				{
					// 筛选规格中 value 以 "I" 开头的产品（INTEL 系列）
					products = products.Where(p => p.Specs.Any(s => s.SkuItems.Any(i => i.Value.StartsWith("I")))).ToList();
				}
				else if (accordion.Equals("AMD", StringComparison.OrdinalIgnoreCase))
				{
					// 筛选规格中 value 以 "R" 开头的产品（AMD 系列）
					products = products.Where(p => p.Specs.Any(s => s.SkuItems.Any(i => i.Value.StartsWith("R")))).ToList();
				}
			}

			return Ok(products);

		}
		[Route("api/products/UpdateLook/{id}")]
		[HttpPost]
		public IHttpActionResult UpdateLook(int id)
		{
			var product = db.Products.FirstOrDefault(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			product.Look += 1;
			db.SaveChanges();

			return Ok();
		}

		[Route("api/products/GetTopProducts")]
		[HttpGet]
		public IHttpActionResult GetTopProducts()
		{
			var topProducts = db.Products
				.Include(p => p.Brand)
				.Include(p => p.ProductCategory)
				.Include(p => p.ProductSkus.Select(s => s.SkuItems))
				.Include(p => p.Images)
				.Where(p => p.Status)
				.OrderByDescending(p => p.Look)
				.Take(10)
				.Select(p => new ProductVm
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					CategoryName = p.ProductCategory.CategoryName,
					Brand = p.Brand.Name,
					Image = p.Images.FirstOrDefault().FileName,
					Look = p.Look,
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
				})
				.ToList();

			return Ok(topProducts);
		}
		[Route("api/products/SaveToCart")]
		[HttpPost]
		public IHttpActionResult SaveCart(CartVm vm)
		{
			using (var db = new AppDbContext())
			{
				
				var existingCartItem = db.Carts
					.FirstOrDefault(c => c.UserId == vm.UserId && c.ProductSkuId == vm.ProductSkuId);

				if (existingCartItem != null)
				{
					
					existingCartItem.Qty += vm.Qty;
				}
				else
				{
					
					var newCartItem = new Cart
					{
						UserId = vm.UserId,
						ProductSkuId = vm.ProductSkuId,
						Qty = vm.Qty
					};
					db.Carts.Add(newCartItem);
				}

				
				db.SaveChanges();

				return Ok("商品已成功加入購物車或商品已經更新");
			}
		}
		[Route("api/products/QueryCart")]
		[HttpGet]
		public IHttpActionResult QueryCart(int UserId, int SkuId)
		{
			using (var db = new AppDbContext())
			{
				// 檢查購物車是否已經存在該商品
				var exists = db.Carts
					.Any(c => c.UserId == UserId && c.ProductSkuId == SkuId);

				
				return Ok(exists);
			}

		}

        [Route("api/products/SaveFollow")]
        [HttpPost]
		public IHttpActionResult SaveFollow(MemberFollowListVm vm)
		{
            using (var db = new AppDbContext())
            {

                var existingFollowItem = db.MemberFollowLists
                    .FirstOrDefault(c => c.UserId == vm.UserId && c.ProductId == vm.ProductId);

                if (existingFollowItem != null)
                {

                    return BadRequest("商品已在追蹤清單");
                }
                else
                {

                    var newFollowItem = new MemberFollowList
                    {
                        UserId = vm.UserId,
                        ProductId = vm.ProductId,
                        Name = vm.Name,
						CreateDate = DateTime.Now
                    };
                    db.MemberFollowLists.Add(newFollowItem);
                }


                db.SaveChanges();

                return Ok("商品已成功加入追蹤清單");
            }
        }

        [Route("api/products/QueryFollow")]
        [HttpGet]
        public IHttpActionResult QueryFollow(int UserId, int ProductId)
		{
            using (var db = new AppDbContext())
            {
                // 檢查追蹤清單是否已經存在該商品
                var exists = db.MemberFollowLists
                    .Any(c => c.UserId == UserId && c.ProductId == ProductId);


                return Ok(exists);
            }
        }
	}
}
