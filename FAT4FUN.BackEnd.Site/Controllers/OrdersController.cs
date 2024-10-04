using AutoMapper;
using FAT4FUN.BackEnd.Site.Models;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

        [MyAuthorize(Functions ="0,1,3")]
        public ActionResult Index()
        {
            List<OrderDto> dto = new OrderRepository().Get();

            List<OrderVm> vm = WebApiApplication._mapper.Map<List<OrderVm>>(dto);

            return View(vm);
        }


        [HttpGet]
        public JsonResult GetOrders()
        {
            List<OrderDto> dto = new OrderRepository().Get();

            List<OrderVm> vm = WebApiApplication._mapper.Map<List<OrderVm>>(dto);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize(Functions = "0,1,3")]
        [HttpPost]
        public ActionResult Update(EditOrderVm vm)
        {

            Result result = HandleUpdageOrder(vm);
            if (result.IsSuccess)
            {
                return RedirectToAction("index");
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(vm);
        }

        private Result HandleUpdageOrder(EditOrderVm vm)
        {
             try
            {
                EditOrderDto dto = WebApiApplication._mapper.Map<EditOrderDto>(vm);
                _orderService.UpdaOrderInfo(dto);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}