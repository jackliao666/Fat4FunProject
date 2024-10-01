using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Interfaces;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models.Services
{
    public class OrderService
    {
        private IOrderRepository _repo;
        public OrderService()
        {
            _repo = new OrderRepository();
        }
        internal void UpdaOrderInfo(EditOrderDto dto)
        {
            OrderDto OrderInDb = _repo.Get(dto.Id);

            //todo 更改規則

            OrderInDb.Status = dto.Status;
            OrderInDb.RecipientName = dto.RecipientName;
            OrderInDb.ShippingAddress = dto.ShippingAddress;
            OrderInDb.Phone = dto.Phone;
            OrderInDb.PaymentMethod = dto.PaymentMethod;
            OrderInDb.ShippingMethod = dto.ShippingMethod;
            OrderInDb.ModifyDate = DateTime.Now;

            _repo.Update(OrderInDb);
        }
    }
}