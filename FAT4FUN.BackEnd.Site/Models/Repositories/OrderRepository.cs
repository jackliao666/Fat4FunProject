using AutoMapper;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
                })
                .OrderBy(o => o.Status) // 先依據狀態排序
                .ThenByDescending(o => o.CreateDate) // 再依據創建日期排序
                .ToList();

            return order;
        }

        public OrderDto Get(int id)
        {
            var order = _db.Orders
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
            if (order == null) return null;

            return WebApiApplication._mapper.Map<OrderDto>(order);
        }

        public void Update(OrderDto dto)
        {
            Order order = WebApiApplication._mapper.Map<Order>(dto);

            //更新 order 到 db

            _db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
    }
}