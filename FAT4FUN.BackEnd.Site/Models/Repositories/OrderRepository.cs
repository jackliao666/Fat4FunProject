using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private AppDbContext _db;

        public OrderRepository()
        {
            _db = new AppDbContext();
        }

        public List<OrderDto> Get()
        {
            var order = _db.Orders.AsNoTracking()
                .OrderBy(o => o.Id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    No = o.No,
                    Status = o.Status,
                    PaymentMethod = o.PaymentMethod,
                    TotalAmount = o.TotalAmount,
                    ShippingMethod = o.ShippingMethod,
                    RecipientName = o.RecipientName,
                    Phone = o.Phone,
                    ShippingAddress = o.ShippingAddress,
                    CreateDate = o.CreateDate,
                    ModifyDate = o.ModifyDate,
                    OrderItems = o.OrderItems.Select(item => new OrderItemDto
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

            return order;
        }
    }
}