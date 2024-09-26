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
                    OrderItems = o.OrderItems.Select(item => new OrderItemVM
                    {
                        Id = item.Id,
                        OrderId = item.OrderId,
                        ProductName = item.ProductName,
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

        //// 可以根據訂單編號查詢特定訂單
        //// GET: api/members/orders/{id}
        //[HttpGet]
        //[Route("api/members/orders/{userid}/{id}")]
        //public IHttpActionResult GetOrderById(int id)
        //{
        //    try
        //    {
        //        var order = db.Orders.Where(o => o.Id == id).Select(o => new OrderVm
        //        {
        //            Id = o.Id,
        //            No = o.No,
        //            PaymentMethod = o.PaymentMethod,
        //            TotalAmount = o.TotalAmount,
        //            Status = o.Status,
        //            CreateDate = o.CreateDate,
        //            OrderItems = o.OrderItems.Select(item => new OrderItemVM
        //            {
        //                Id = item.Id,
        //                OrderId = item.OrderId,
        //                ProductName = item.ProductName,
        //                Price = item.Price,
        //                Qty = item.Qty,
        //                SubTotal = item.SubTotal
        //            }).ToList()
        //        }).FirstOrDefault();

        //        if (order == null)
        //        {
        //            return NotFound(); // 返回 404 Not Found
        //        }

        //        return Ok(order); // 返回 200 OK 和特定訂單資料
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex); // 返回 500 Internal Server Error
        //    }
        //}

    }
}
