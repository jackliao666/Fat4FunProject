using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;

        public ProductsController()
        {
            _productService = new ProductService();
        }
        // GET: Products

        public ActionResult Index()
        {
            List<ProductVm> products = _productService.GetAll();
            return View(products);
        }
        [HttpGet]
        public ActionResult Create()
        {
            //var categories = _productService.GetCategories(); // 假設有方法獲取所有類別
            //var brands = _productService.GetBrands(); // 假設有方法獲取所有品牌

            //ViewBag.Categories = new SelectList(categories, "Id", "Name");
            //ViewBag.Brands = new SelectList(brands, "Id", "Name");

            return View(new ProductVm());
        }

        // 新增商品，提交表單時觸發
        [HttpPost]
        public ActionResult Create(ProductVm vm)
        {
            if (ModelState.IsValid)
            {
                _productService.Create(vm);
                return RedirectToAction("Index");
            }
            return View(vm);
        }
    }
}