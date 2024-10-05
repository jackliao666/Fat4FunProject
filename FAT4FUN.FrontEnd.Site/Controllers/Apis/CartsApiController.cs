using FAT4FUN.FrontEnd.Site.Models.Dto;
using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

        [HttpPut]
        [Route("api/carts/UpdateItemQty")]
        public IHttpActionResult UpdateItemQty(CartDto dto)
        {
            //取得目前購物車主檔，若沒有，就立刻新增一筆並傳回
            var cart = GetCartInfo(dto.UserId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => 
                                ci.ProductSkuId == dto.ProductSkuId &&
                            ci.SkuItemId == dto.SkuItemId);
            if (cartItem == null) return BadRequest("請提供正確的購物車和商品資訊");
            var newQty = dto.Qty;
            
            try
            {                             

                if (cartItem == null) return NotFound(); // 如果找不到對應的購物車項目，返回404

                // 更新購物車項目的數量
                if (newQty == 0)
                {
                    var entity = db.Carts.FirstOrDefault(ci => ci.Id == cartItem.Id);
                    if (entity != null)
                    {
                        db.Carts.Remove(entity);
                        db.SaveChanges(); // 確保保存變更
                    }

                }
                else
                {
                    // 否則更新數量
                    var cartItemInDb = db.Carts.FirstOrDefault(ci => ci.Id == cartItem.Id);
                    cartItemInDb.Qty = newQty;  
                    // 保存變更
                    db.SaveChanges();
                }

                return Ok("商品數量已更新");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 錯誤
            }
        }

        [HttpDelete]
        [Route("api/carts/RemoveItem")]
        public IHttpActionResult RemoveItem(CartDto dto)
        {
            try
            {
                // 找到對應的購物車項目
                var cartItem = db.Carts.FirstOrDefault(ci => ci.UserId == dto.UserId &&
                                                              ci.ProductSkuId == dto.ProductSkuId &&
                                                              ci.SkuItemId == dto.SkuItemId);

                if (cartItem == null)
                {
                    return NotFound(); // 如果找不到對應的購物車項目，返回404
                }

                // 從數據庫中刪除該項目
                db.Carts.Remove(cartItem);
                db.SaveChanges(); // 確保保存變更

                return Ok("商品已成功從購物車中移除");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 錯誤
            }
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
                            Price = c.ProductSku.Sale,
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
            var total = cartInfo.Sum(item => item.SubTotal);

            return (cartInfo, total); // 返回購物車項目和總金額

        }

        //==============以下是建立訂單=========

        [HttpPost]
        [Route("api/carts/SubmitOrder")]
        public IHttpActionResult SubmitOrder([FromBody] CheckoutOrderRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest("請求數據無效");
            }
            var userid = request.UserId;
            var vm = request.Vm;
            //var account = User.Identity.Name;
            var cart = GetCartInfo(userid);

            //if (!ModelState.IsValid)
            //{
            //    // 重新載入購物車明細，因為表單驗證失敗時需要顯示購物車資訊
            //    vm.Cart = cart.CartItems;  // 確保購物車明細仍然顯示在表單中
            //    return Ok(vm);
            //}
 
            //vm.Cart = cart.CartItems;
            if (!cart.CartItems.Any())
            {
                ModelState.AddModelError(string.Empty, "購物車是空的，無法結帳");
                //vm.Cart = cart.CartItems;
                return Ok(vm);
            }


            try
            {
                ProcessCheckout(userid, vm);
                return Ok("Checkout-Success");
            }
            catch (DbUpdateException)
            {
                return BadRequest("結帳失敗，請稍後再試。");
            }
            catch (Exception ex)
            {
                // 記錄錯誤
                return InternalServerError(ex);
            }
        }

        private void ProcessCheckout(int userid, CheckoutVm vm)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try { 
                //建立訂單主黨明細檔
                CreateOrder(userid, vm);

                ////清空購物車
                var cartItems = db.Carts
                         .Where(c => c.UserId == userid)
                         .ToList(); // 不用 AsNoTracking，因為需要刪除實體
                if (!cartItems.Any()) return;

                // 刪除每個購物車項目
                db.Carts.RemoveRange(cartItems);


                // 保存變更
                db.SaveChanges();

                transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // 處理例外情況，可能需要記錄錯誤或返回錯誤信息
                    throw new Exception("結帳失敗，請稍後再試。", ex);
                }
            }

        }

        
        private void CreateOrder(int userid, CheckoutVm vm)
        {
            var userId = db.Users.First(m => m.Id == userid).Id;

            //取得Cart資料
            var cartinfo = GetCartInfo(userId);
           
            //新增訂單主檔
            var order = new Order
            {
                UserId = userId,
                No= GenerateOrderNo(userId), // 使用生成的訂單編號,
                PaymentMethod =1,
                TotalAmount = cartinfo.Total,
                ShippingMethod=1,
                RecipientName=vm.RecipientName,
                ShippingAddress=vm.ShippingAddress,
                Phone=vm.Phone,
                Status = 1,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
            };

            //新增訂單明細檔
            foreach (var item in cartinfo.CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductSkuId = item.ProductSkuId,
                    ProductName = item.Product.Name,
                    SkuItemId=item.SkuItemId ?? null,
                    SkuItemName =item.SkuItem?.Value ?? "基本款",
                    Price = item.Product.Price + (item.SkuItem?.SkuPrice ?? 0),
                    Qty = item.Qty,
                    SubTotal = item.SubTotal,
                });
            }
            db.Orders.Add(order);
            db.SaveChanges();

        }

        private string GenerateOrderNo(int userId)
        {
            // 獲取當前日期並轉為所需的格式，例如 "20240924"
            string currentDate = DateTime.Now.ToString("yyyyMMdd");

            // 獲取當天的開始和結束時間
            DateTime startDate = DateTime.Today; // 當天的開始時間
            DateTime endDate = startDate.AddDays(1); // 當天的結束時間

            // 查詢當天該用戶的訂單數量來生成流水號
            int orderCountToday = db.Orders
                                    .Where(o => o.UserId == userId && o.CreateDate >= startDate && o.CreateDate < endDate)
                                    .Count() + 1; // 當天已有訂單數量 +1 作為流水號

            // 格式化訂單編號，例如 "20240924-1001-001"
            string orderNo = $"{currentDate}{userId:D3}{orderCountToday:D3}";

            return orderNo;
        }

        public class CheckoutOrderRequest
        {
            public int UserId { get; set; }
            public CheckoutVm Vm { get; set; }
        }

    }

}
