using FAT4FUN.BackEnd.Site.Models;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Controllers
{
    public class ProductSkusController : Controller
    {
        private readonly ProductSkuService _productSkuService;


        public ProductSkusController()
        {
            _productSkuService = new ProductSkuService();
        }
        // GET: ProductSkus
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Index()
        {
            var productVm = _productSkuService.GetProducts();
            return View(productVm);
        }
       
        [MyAuthorize(Functions = "0,1,2")]

        public ActionResult Create()
        {
            var productSku = _productSkuService.GetProductSkus();

            ViewBag.ProductSku = productSku;
            
            var productVm = new Product2Vm
            {
                ProductSku = new ProductSkuVm
                {
                    SkuItems = new List<SkuItemVm>()               
                }
            };

            return View(productVm);
        }

        [HttpPost]
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Create(Product2Vm vm)
        {
            if (ModelState.IsValid)
            {
                var productDto = WebApiApplication._mapper.Map<Product2Dto>(vm);

                var productSkuId = _productSkuService.Create(productDto);

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Create2(int id)
        {
            Product2Vm product = _productSkuService.GetProducts(id);

            // 如果產品不存在，返回 404 錯誤
            if (product == null)
            {
                return HttpNotFound();
            }
           

            return View(product);

        }

        [HttpPost]
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Create2(Product2Vm vm)
        {
            if (ModelState.IsValid)
            {
                var productDto = WebApiApplication._mapper.Map<Product2Dto>(vm);

                var productSkuId = _productSkuService.Create(productDto);

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Edit(int id)
        {
            Product2Vm product = _productSkuService.GetProducts(id);
            var skuItem = _productSkuService.GetSkuItems(id);

            // 如果產品不存在，返回 404 錯誤
            if (product == null)
            {
                return HttpNotFound();
            }
            

            ViewBag.SkuItems = skuItem;



            return View(product);
        }

        [HttpPost]
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Edit(Product2Vm vm)
        {
            if (ModelState.IsValid)
            {
                var productDto = WebApiApplication._mapper.Map<Product2Dto>(vm);

                var productSkuId = _productSkuService.Update(productDto);

                return RedirectToAction("Index");
            }
            ViewBag.SkuItems = _productSkuService.GetSkuItems(vm.Id);

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Delete(int productId,int skuId,int skuItemId)
        {
            _productSkuService.Delete(productId, skuId, skuItemId);

            return RedirectToAction("Index");
        }
    }
}