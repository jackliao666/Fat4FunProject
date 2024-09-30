using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderService _orderService;

        public OrdersController()
        {
            _orderService = new OrderService();
        }

        // GET: Orders
        public ActionResult Index()
        {
            List<OrderDto> dto = new OrderRepository().Get();

            List<OrderVm> vm = WebApiApplication._mapper.Map<List<OrderVm>>(dto);

            return View(vm);
        }
    }
}