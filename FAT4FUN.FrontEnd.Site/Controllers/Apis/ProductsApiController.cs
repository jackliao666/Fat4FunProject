using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.IO;
using System.Web;





namespace FAT4FUN.FrontEnd.Site.Controllers.Apis
{

	public class ProductsApiController : ApiController
	{
		private AppDbContext db = new AppDbContext();

		[Route("api/products/GetSingleProduct")]
		[HttpGet]
		public IHttpActionResult GetSingleProduct(int id)
		{
			var products = db.Products
				.Include(p => p.Brand)
				.Include(p => p.ProductCategory)
				.Include(p => p.ProductSkus.Select(s => s.SkuItems))
				.Include(p => p.Images)
				.Where(p => p.Status && p.Id == id)
				.Select(p => new ProductVm
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					CategoryName = p.ProductCategory.CategoryName,
					Brand = p.Brand.Name,
					Image = p.Images.FirstOrDefault().Path,
					ImageList = p.Images.Select(i => new ProdctImageVm
					{
						Id = i.Id,
						Path = i.Path,
						Sort = i.Sort
					}).ToList(),
					Look = p.Look,
					Specs = p.ProductSkus.Select(s => new ProductSkuVm
					{
						Id = s.Id,
						Name = s.Name,
						Price = s.Price,
						Sale = s.Sale,
						SkuItems = s.SkuItems.Select(i => new SkuItemVm
						{
							Id = i.Id,
							Key = i.key,
							Value = i.value,
							SkuPrice = i.Price
						}).ToList()
					}).ToList()
				}).FirstOrDefault();

			return Ok(products);
		}


		// GET: api/Products
		//取得所有商品
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
					Image = p.Images.FirstOrDefault().Path, // 假設你只需要第一張圖片
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
			//手風琴設定
			if (!string.IsNullOrEmpty(accordion))
			{
				if (accordion.Equals("INTEL", StringComparison.OrdinalIgnoreCase))
				{
					products = products.Where(p => p.Specs.Any(s => s.SkuItems.Any(i => i.Value.StartsWith("I")))).ToList();
				}
				else if (accordion.Equals("AMD", StringComparison.OrdinalIgnoreCase))
				{
					products = products.Where(p => p.Specs.Any(s => s.SkuItems.Any(i => i.Value.StartsWith("R")))).ToList();
				}

			}
			

            return Ok(products);
		}

		[Route("api/products/GetListProducts")]
		[HttpGet]
		public IHttpActionResult GetListProducts(int? maxPrice = null, int? minPrice = null, string brand = null,
			string categoryName = null, string key = null, string value = null, string sort = null, string accordion = null)
		{
			var products = db.Products
				.Include(p => p.Brand)
				.Include(p => p.ProductCategory)
				.Include(p => p.ProductSkus.Select(s => s.SkuItems))
				.Include(p => p.Images)
				.Where(p => p.Status) // 只取得啟用的產品
				.Where(p => p.ProductCategory.CategoryName != "PC")
				.Select(p => new ProductVm
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					CategoryName = p.ProductCategory.CategoryName,
					Brand = p.Brand.Name,
					Image = p.Images.FirstOrDefault().Path, // 假設你只需要第一張圖片
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
				});

			if (!string.IsNullOrEmpty(categoryName))
			{
				products = products.Where(p => p.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
			}
			if (!string.IsNullOrEmpty(brand))
			{
				products = products.Where(p => p.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
			}
			if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
			{
				products = products.Where(p => p.Specs.Any(s => s.SkuItems.Any(i => i.KeyList.Contains(key) && i.ValueList.Contains(value))));
			}
			if (maxPrice.HasValue)
			{
				products = products.Where(p => p.Specs.Any(s => s.Price <= maxPrice.Value));
			}
			if (minPrice.HasValue)
			{
				products = products.Where(p => p.Specs.Any(s => s.Price >= minPrice.Value));
			}
			// 排序邏輯
			if (string.IsNullOrEmpty(sort) || sort == "name")
			{
				// 默認按照名稱排序
				products = products.OrderBy(p => p.Name);
			}
			else if (sort == "price_asc")
			{
				// 按價格升序排序
				products = products.OrderBy(p => p.Specs.Min(s => s.Price));
			}
			else if (sort == "price_desc")
			{
				// 按價格降序排序
				products = products.OrderByDescending(p => p.Specs.Max(s => s.Price));
			}		
			// 定義字典來映射手風琴設定的條件
			var filterMap = new Dictionary<string, (string Brand, string CategoryName)>
			{
				{ "羅技鍵盤", ("羅技", "鍵盤") },
				{ "羅技滑鼠", ("羅技", "滑鼠") },
				{ "羅技方向盤", ("羅技", "方向盤") },
				{ "海盜船鍵盤", ("海盜船", "鍵盤") },
				{ "雷蛇鍵盤", ("雷蛇", "鍵盤") },
				{ "雷蛇滑鼠", ("雷蛇", "滑鼠") },
				//{ "雷蛇耳機", ("雷蛇", "耳機") },
                { "酷睿處理器", ("INTEL", "處理器") },
				{ "銳龍處理器", ("AMD", "處理器") },
                { "銳龍顯示卡", ("AMD", "顯示卡") },
                { "金士頓記憶體", ("金士頓", "記憶體") },
				{ "美光記憶體", ("美光", "記憶體") },
				{ "美光硬碟", ("美光", "硬碟") }
			};

			if (!string.IsNullOrEmpty(accordion))
			{
				// INTEL 和 AMD 特殊處理
				if (accordion.Equals("INTEL", StringComparison.OrdinalIgnoreCase))
				{
					products = products.Where(p => p.Specs.Any(s => s.SkuItems.Any(i => i.Value.StartsWith("I"))));
				}
				else if (accordion.Equals("AMD", StringComparison.OrdinalIgnoreCase))
				{
					products = products.Where(p => p.Specs.Any(s => s.SkuItems.Any(i => i.Value.StartsWith("R"))));
				}

				else if (filterMap.ContainsKey(accordion))
				{
					var (Brand, category) = filterMap[accordion];

					// 修改條件，要求品牌和分類必須相等
					products = products.Where(p => p.Brand.Equals(Brand, StringComparison.OrdinalIgnoreCase)
												&& p.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase));
				}
			}
			return Ok(products.ToList());
		}

		//取得卡片的點擊次數
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

		//取得熱門商品
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
					Image = p.Images.FirstOrDefault().Path,
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
				})
				.ToList();

            return Ok(topProducts);
		}

		//存購物車
		[Route("api/products/SaveToCart")]
		[HttpPost]
		public IHttpActionResult SaveCart(CartVm vm)
		{
			using (var db = new AppDbContext())
			{
				var existingCartItem = db.Carts
					.FirstOrDefault(c => c.UserId == vm.UserId && c.ProductSkuId == vm.ProductSkuId && c.SkuItemId == vm.SkuItemId);

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
						SkuItemId = vm.SkuItemId,
						Qty = vm.Qty
					};
					db.Carts.Add(newCartItem);
				}

				db.SaveChanges();

				return Ok("商品已成功加入購物車或商品已經更新");
			}
		}
		//查詢購物車
		[Route("api/products/QueryCart")]
		[HttpGet]
		public IHttpActionResult QueryCart(int UserId, int ProductSkuId, int? SkuItemId)
		{
			using (var db = new AppDbContext())
			{
				// 檢查購物車是否已經存在該商品


				var exists = db.Carts
					.Any(c => c.UserId == UserId && c.ProductSkuId == ProductSkuId && c.SkuItemId == SkuItemId);
				return Ok(exists);
			}

		}
		//存追蹤清單
		[Route("api/products/SaveFollow")]
		[HttpPost]
		public IHttpActionResult SaveFollow(MemberFollowListVm vm)
		{
			using (var db = new AppDbContext())
			{
				var existingFollowItem = db.MemberFollowLists
					.Any(c => c.UserId == vm.UserId && c.ProductId == vm.ProductId);

				if (existingFollowItem)
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
		//查詢追蹤清單
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
