using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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



    }
}
