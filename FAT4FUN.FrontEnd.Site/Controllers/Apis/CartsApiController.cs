using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Web.Http;

namespace FAT4FUN.FrontEnd.Site.Controllers.Apis
{
    public class CartsApiController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        [HttpGet]
        [Route("api/carts/{userid}")]
        public IHttpActionResult GetCart(int userid)
        {
            try
            {
                var (cartInfo, total) = GetCartInfo(userid); // 更新為 Tuple 返回 CartItems 和 Total

                if (cartInfo.Count == 0)
                {
                    return NotFound(); // 如果沒有找到購物車，返回 404
                }

                return Ok(new { CartItems = cartInfo, Total = total }); // 返回 CartItems 和 Total
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 錯誤
            }
        }


        private void AddCart(string account, int productSkuId, int qty, int? skuItemId = null, int? cartId = null)
        {
            if (!cartId.HasValue)
            {
                cartId = CreateNewCart(account);  // 使用帳號創建新的購物車
            }
            AddCartItem(cartId.Value, productSkuId, qty, skuItemId);
        }

        private int CreateNewCart(string account)
        {
            var db = new AppDbContext();

            var userId = GetUserIdByAccount(account); // 根據帳號獲取使用者 ID
            var newCart = new Cart
            {
                UserId = userId
                // 可視需要添加其他初始化的屬性
            };

            db.Carts.Add(newCart);
            db.SaveChanges();

            return newCart.Id; // 返回新的購物車 Id
        }

        private int GetUserIdByAccount(string account)
        {
            var user = db.Users.FirstOrDefault(u => u.Account == account);
            if (user == null)
            {
                throw new Exception("使用者不存在");
            }
            return user.Id;
        }

        private void AddCartItem(int cartId, int productSkuId, int qty, int? skuItemId)
        {
            // 檢查購物車中是否已經有該商品及指定規格
            var cartItem = db.Carts
                .FirstOrDefault(c => c.UserId == db.Carts.Find(cartId).UserId &&
                                     c.ProductSkuId == productSkuId &&
                                     c.SkuItemId == skuItemId);

            if (cartItem == null)
            {
                // 如果購物車中沒有該商品或該規格，則新增一個項目
                cartItem = new Cart
                {
                    UserId = db.Carts.Find(cartId).UserId,
                    ProductSkuId = productSkuId,
                    SkuItemId = skuItemId,
                    Qty = qty,
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                // 如果商品和規格已經存在，則更新數量
                cartItem.Qty += qty;
            }

            // 儲存變更
            db.SaveChanges();
        }

        /// <summary>
        /// 取得目前購物車主檔，若沒有，就立刻新增一筆傳回
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private (List<CartVm> CartItems, int Total) GetCartInfo(int userid)
        {

            var cartInfo = db.Carts
                    .Where(c => c.UserId == userid)
                    .AsNoTracking()
                    .Select(c => new CartVm
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        ProductSkuId = c.ProductSkuId,
                        SkuItemId = c.SkuItemId,
                        Qty = c.Qty,
                        Product = new ProductSkuVm
                        {
                            Id = c.ProductSku.Id,
                            Name = c.ProductSku.Name,
                            Price = c.ProductSku.Price,
                        },
                        SkuItem = c.SkuItemId != null ? new SkuItemVm
                        {
                            Key =c.SkuItem.key,
                            Value = c.SkuItem.value,
                            SkuPrice = c.SkuItem.Price,
                        } : null,
                    }).ToList();


            // 如果購物車項目為空，可以選擇創建一個新的購物車
            if (!cartInfo.Any())
            {
                var newCart = new Cart { UserId = userid };
                db.Carts.Add(newCart);
                db.SaveChanges();
                return (new List<CartVm>(), 0); // 返回空購物車和總金額 0
            }

            // 計算總金額
            var total = cartInfo.Sum(item => item.SubTotal) ?? 0;

            return (cartInfo, total); // 返回購物車項目和總金額

        }
    }
}
