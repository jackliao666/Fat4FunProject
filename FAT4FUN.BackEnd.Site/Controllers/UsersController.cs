using FAT4FUN.BackEnd.Site.Models;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Controllers
{
    public class UsersController : Controller
    {
        
        private readonly UserService _service;

        public UsersController()
        {
            _service = new UserService();
        }

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVm vm)
        {
            if (ModelState.IsValid)
            {
                Result result = _service.HandleLogin(vm);
                if (result.IsSuccess)
                {
                    (string url, HttpCookie cookie) = ProcessLogin(vm.Account);
                    Response.Cookies.Add(cookie);
                    return Redirect(url);
                }

                ModelState.AddModelError(
                    string.Empty,
                    result.ErrorMessage);
            }
            return View(vm);
        }




    }
}
