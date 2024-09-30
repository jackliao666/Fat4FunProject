using FAT4FUN.FrontEnd.Site.Models.Dto;
using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FAT4FUN.FrontEnd.Site.Controllers.Apis
{

    public class MembersApiController : ApiController
    {
        private AppDbContext db = new AppDbContext();
        
        [HttpGet]
        [Route("api/Members/Orders/{userid}")]
        public IHttpActionResult GetOrders(int userid)
        {
            try
            {
                // 查詢所有訂單
                var orders = db.Orders
                    .Where(c => c.UserId == userid)
                    .AsNoTracking()
                    .Select(o => new OrderVm
                {
                    Id = o.Id,
                    No = o.No,
                    PaymentMethod = o.PaymentMethod,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreateDate = o.CreateDate,
                    ModifyDate = o.ModifyDate,
                    OrderItems = o.OrderItems.Select(item => new OrderItemVM
                    {
                        Id = item.Id,
                        OrderId = item.OrderId,
                        ProductName = item.ProductName,
                        SkuItemName = item.SkuItemName ?? "基本款",
                        Price = item.Price,
                        Qty = item.Qty,
                        SubTotal = item.SubTotal
                    }).ToList()
                }).ToList();

                return Ok(orders); // 返回 200 OK 和訂單資料
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 Internal Server Error
            }
        }

        [HttpPut]
        [Route("api/Members/Orders/SubmitReturnRequest/{orderId}")]
        public IHttpActionResult SubmitReturnRequest(int orderId)
        {
            try
            {
                // 確認請求的訂單ID是否有效
                var order = db.Orders.Find(orderId);
                if (order == null)
                {
                    return NotFound(); // 如果訂單不存在，返回 404 Not Found
                }

                // 更新訂單狀態為「已申請退貨」(4)
                order.Status = 4;
                
                string originalModifyDate = order.ModifyDate.ToString("yyyy-MM-dd");
                
                // 更新 modifyDate 為當前日期
                order.ModifyDate = DateTime.Now;

                db.SaveChanges();

                return Ok(new
                {
                    message = "退貨申請已成功提交。",
                    originalModifyDate = originalModifyDate
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 Internal Server Error
            }
        }

        [HttpPut]
        [Route("api/Members/Orders/CancelOrder/{orderId}")]
        public IHttpActionResult CancelOrder(int orderId)
        {
            try
            {
                // 確認請求的訂單ID是否有效
                var order = db.Orders.Find(orderId);
                if (order == null)
                {
                    return NotFound(); // 如果訂單不存在，返回 404 Not Found
                }

                order.Status = 6;

                string originalModifyDate = order.ModifyDate.ToString("yyyy-MM-dd");

                // 更新 modifyDate 為當前日期
                order.ModifyDate = DateTime.Now;

                db.SaveChanges();

                return Ok(new { message = "取消訂單申請已成功提交。" }); // 返回成功訊息
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 Internal Server Error
            }
        }

        [HttpGet]
        [Route("api/Members/Track/{userid}")]
        public IHttpActionResult GetTrack(int userId)
        {
            try
            {
                // 查詢所有追蹤商品
                var MemberFollowLists = db.MemberFollowLists
                    .Where(c => c.UserId == userId)
                    .AsNoTracking()
                    .Select(f => new MemberFollowListVm
                    {
                        Id=f.Id,
                        UserId=f.UserId,
                        ProductId=f.ProductId,
                        ProductSkuId= f.Product.ProductSkus
                        .Select(sku => sku.Id)
                        .FirstOrDefault(),
                        Name = f.Product.Name,
                        Price = f.Product.ProductSkus
                        .Select(sku => sku.Sale)
                        .FirstOrDefault(), 
                        Image=f.Product.Images.FirstOrDefault().Path,// 假設你只需要第一張圖片
                        CreateDate =f.CreateDate          
                    }).ToList();

                return Ok(MemberFollowLists); // 返回 200 OK 和訂單資料
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 Internal Server Error
            }
        }

        //存購物車
        [Route("api/Track/SaveToCart")]
        [HttpPost]
        public IHttpActionResult SaveCart(CartVm vm)
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

                return Ok("商品已成功加入購物車");
        }

        //查詢購物車
        [Route("api/Track/QueryCart")]
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

        [HttpDelete]
        [Route("api/Members/Track/RemoveItem/{memberFollowListsId}")]
        public IHttpActionResult RemoveItem(int memberFollowListsId)
        {
            try
            {
                // 找到對應的購物車項目
                var MemberFollowListsitem = db.MemberFollowLists.FirstOrDefault(ci => ci.Id == memberFollowListsId );

                if (MemberFollowListsitem == null)
                {
                    return NotFound(); // 如果找不到對應的購物車項目，返回404
                }

                // 從數據庫中刪除該項目
                db.MemberFollowLists.Remove(MemberFollowListsitem);
                db.SaveChanges(); // 確保保存變更

                return Ok("商品已成功從追蹤清單中移除");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 返回 500 錯誤
            }
        }



    }
}
