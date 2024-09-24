using FAT4FUN.FrontEnd.Site.Services;
using FAT4FUN.FrontEnd.Site.Services.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.FrontEnd.Site.Controllers
{
	public class ProductController : Controller
	{
		// GET: Product
		public ActionResult Index()
		{

			ProductService productService = new ProductService();

			var data = productService.GetProduct(1);




			return View();
		}
	}
}