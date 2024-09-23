using FAT4FUN.BackEnd.Site.Models;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FAT4FUN.BackEnd.Site.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Register()
        {
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult Register(RegisterVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Result result = HandleRegister(vm);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(vm);
            }


            return View("RegisterConfirm"); // 顯示 RegisterConfirm 網頁內容,不必有action
            // return RedirectToAction("RegisterConfirm"); //如果想要轉到某 action 就這麼寫

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
                Result result = HandleLogin(vm);
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

        public ActionResult Logout() 
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login","Users");
        }

        public ActionResult ActiveRegister(int? userId, string confirmCode)
        {
            if (!userId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "User ID is required.");
                return View();
            }

            Result result = HandleActiveRegister(userId.Value, confirmCode);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View();
            }

            return View();
        }


        private (string url, HttpCookie cookie) ProcessLogin(string account)
        {
            var roles = string.Empty; // 沒有用到角色權限,所以存入空白
            
            // 建立一張認證票
            var ticket =
                new FormsAuthenticationTicket(
                    1,                          //版本號
                    account,                    //
                    DateTime.Now,               //發行日
                    DateTime.Now.AddMinutes(30),    //到期日
                    false,                      //是否續存
                    roles,                      //userData
                    "/"                         //cookie位置
                    );
            
            //將它加密
            var value = FormsAuthentication.Encrypt(ticket);

            //存入cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

            //取得 return url
            var url = FormsAuthentication.GetRedirectUrl(account, true);   //第二個引述沒有用處
            return (url, cookie);

        }

        private Result HandleLogin(LoginVm vm)
        {
            try 
            { 
                var service = new UserService();
                LoginDto dto = WebApiApplication._mapper.Map<LoginDto>(vm);

                Result validateResult = service.ValidateLogin(dto);
                return validateResult;
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }


        private Result HandleActiveRegister(int userId, string confirmCode)
        {
            try
            {
                var service = new UserService();
                service.ActiveRegister(userId, confirmCode);

                return Result.Success();
            }
            catch (Exception ex)
            {
                // 记录错误信息以便调试
                System.Diagnostics.Debug.WriteLine($"Error activating user: {ex.Message}");
                return Result.Fail(ex.Message);
            }
        }

        private Result HandleRegister(RegisterVm vm)
        {

            //可自行決定叫用EF or Service object進行 create user 的工作
            UserService service = new UserService();

            try
            {
                RegisterDto dto = vm.ToDto();
                service.Register(dto);

                return Result.Success();
            }
            //catch (DbEntityValidationException ex)
            //{
            //    // 获取详细的验证错误信息
            //    var validationErrors = ex.EntityValidationErrors
            //        .SelectMany(e => e.ValidationErrors)
            //        .Select(e => $"Property: {e.PropertyName}, Error: {e.ErrorMessage}")
            //        .ToList();

            //    // 记录错误信息或打印出来
            //    foreach (var error in validationErrors)
            //    {
            //        System.Diagnostics.Debug.WriteLine(error); // 在调试窗口打印错误
            //    }

            //    // 返回详细的验证错误信息
            //    return Result.Fail(string.Join(", ", validationErrors));
            //}
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}