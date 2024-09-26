using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            List<ProductVm> products = _productService.GetProducts();
            return View(products);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var categories = _productService.GetCategories(); // 獲取所有分類
            var brands = _productService.GetBrands(); // 獲取所有品牌

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;

            var productVm = new ProductVm
            {
                ProductSkus = new ProductSkuVm()// 初始化列表
            };

            return View(productVm);
        }

        // 新增商品，提交表單時觸發
        [HttpPost]
        public ActionResult Create(ProductVm vm)
        {
            if (ModelState.IsValid)
            {
                // 將 ProductVm 轉換為 ProductDto
                var productDto = WebApiApplication._mapper.Map<ProductDto>(vm);

                var createdProductId = _productService.Create(productDto);

                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProductVm product = _productService.GetProduct(id);
                
            // 如果產品不存在，返回 404 錯誤
            if (product == null)
            {
                return HttpNotFound();
            }
            var categories = _productService.GetCategories(); // 獲取所有分類
            var brands = _productService.GetBrands(); // 獲取所有品牌

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;

            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(ProductVm vm)
        {
            if (ModelState.IsValid)
            {
                var productDto = WebApiApplication._mapper.Map<ProductDto>(vm);
                _productService.UpdateProduct(productDto);
                // 更新資料邏輯
                // Save changes to the database
                return RedirectToAction("Index");
            }
            return View(vm);
        }

    }
}