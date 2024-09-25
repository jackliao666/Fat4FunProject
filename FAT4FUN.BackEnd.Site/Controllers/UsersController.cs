using FAT4FUN.BackEnd.Site.Models;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Register()
        {
            var roles = new List<SelectListItem>
    {
        new SelectListItem { Value = "0", Text = "Admin" },
        new SelectListItem { Value = "1", Text = "Manager" },
        new SelectListItem { Value = "2", Text = "Designer" },
        new SelectListItem { Value = "3", Text = "Sales" },
        new SelectListItem { Value = "4", Text = "Human Resources" },
        new SelectListItem { Value = "5", Text = "Members" }
    };

            ViewBag.Roles = roles;
            return View();
        }



        [MyAuthorize(Functions = "0,3")]
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
            /*return RedirectToAction("RegisterConfirm");*/ //如果想要轉到某 action 就這麼寫

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
                var service = new UserService();
                Result result = HandleLogin(vm);

                if (result.IsSuccess)
                {
                    var roleResult = service.GetUserRole(vm.Account);

                    if (!roleResult.IsSuccess)
                    {
                        // 如果獲取角色失敗，顯示錯誤訊息
                        ModelState.AddModelError(string.Empty, roleResult.ErrorMessage);
                        return View(vm);
                    }
                    
                    // 獲取角色數據
                    int userRole = (int)roleResult.Data;

                    // 根據角色處理邏輯
                    // 例如，將角色數據寫入 cookie 或其他處理邏輯
                    (string url, HttpCookie cookie) = ProcessLogin(vm.Account);
                    Response.Cookies.Add(cookie);
                }

                ModelState.AddModelError(
                    string.Empty,
                    result.ErrorMessage);
            }
            return View("Index");
        }

        public ActionResult Logout() 
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login","Users");
        }

        public ActionResult ActiveRegister(int? userId, string confirmCode)
        {

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
            //var roles = string.Empty; // 沒有用到角色權限,所以存入空白
            var service = new UserService();
            int roleNumber = service.GetUserRole(account).Data as int? ?? 0; //取得角色數字(0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20 


            // 建立一張認證票
            var ticket =
                new FormsAuthenticationTicket(
                    1,                          //版本號
                    account,                    //
                    DateTime.Now,               //發行日
                    DateTime.Now.AddMinutes(30),    //到期日
                    false,                      //是否續存
                    roleNumber.ToString(),      // 將角色數字存入 UserData
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

            //try
            //{
            //    RegisterDto dto = vm.ToDto();
            //    service.Register(dto);
            //    service.AssignRole(dto.Id, dto.Role);

            //    return Result.Success();
            //}

            //catch (Exception ex)
            //{
            //    return Result.Fail(ex.Message);
            //}
            try
            {
                RegisterDto dto = vm.ToDto(); // 將 ViewModel 轉換為 DTO
                service.Register(dto);         // 呼叫服務層進行註冊操作

                // 為使用者分配選中的角色
                foreach (var role in vm.Roles)
                {
                    service.AssignRole(dto.Id, role); // 為使用者分配每個選中的角色
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

        }
    }
}
