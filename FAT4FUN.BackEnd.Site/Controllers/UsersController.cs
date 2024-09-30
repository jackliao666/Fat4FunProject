using FAT4FUN.BackEnd.Site.Models;
using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.EFModels;
using FAT4FUN.BackEnd.Site.Models.Repositories;
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


        private List<SelectListItem> GetRoles()
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

            return roles;
        }

        [MyAuthorize(Functions ="0,4")]
        public ActionResult Register()
        {
            // 設置 Roles 選項
            var roles = GetRoles();

            ViewBag.Roles = new SelectList(roles, "Value", "Text");
            return View();
        }



        [MyAuthorize(Functions ="0,4")]
        [HttpPost]
        public ActionResult Register(RegisterVm vm)
        {
            // 測試用的解密程式碼
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                // 輸出解密後的角色資料
                System.Diagnostics.Debug.WriteLine($"User Role from Decrypted Cookie: {ticket.UserData}");
            }



            // 重新設置 Roles 選項
            var roles = GetRoles();

            ViewBag.Roles = new SelectList(roles, "Value", "Text");

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
            if (!ModelState.IsValid)
            {

                return View(vm);
            }
            
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
                    
                    
                        
                    
                    //// 設置身份驗證的 Cookie，這行是關鍵
                    //FormsAuthentication.SetAuthCookie(vm.Account, true);

                    // 登入成功後的重定向
                    return RedirectToAction("EditProfile", "Users");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    return View(vm);
                }

                
            
        }

        public ActionResult Logout() 
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login","Users");
        }

        
        [Authorize]
        public ActionResult EditProfile()
        {
            var account = User.Identity.Name;
            UserDto dto = new UserRepository().Get(account);

            EditProfileVm vm = WebApiApplication._mapper.Map<EditProfileVm>(dto);
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(EditProfileVm vm)
        {
            string account = User.Identity.Name;
            Result result = HandleUpdateProfile(account, vm);

            if (result.IsSuccess) 
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(vm);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVm vm)
        {
            string account = User.Identity.Name;
            Result result = HandleChangePassword(account, vm);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index"); // 密碼修改成功，轉到首頁
            }
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(vm); // 密碼修改失敗，返回修改密碼頁面

        }

        [HttpPost]
        public JsonResult ToggleUserStatus(int id, bool status)
        {
            try
            {
                var service = new UserService();
                service.UpdateUserStatus(id, status); // 使用你定義好的服務方法來更新狀態
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // 可以記錄錯誤訊息
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult UserCheck()     
        {
            var service = new UserService();
            var users = service.GetAllUser();
            return View(users);
        }





        private Result HandleChangePassword(string account, ChangePasswordVm vm)
        {
            var service = new UserService();
            try 
            {
                ChangePasswordDto dto = WebApiApplication._mapper.Map<ChangePasswordDto>(vm);
                service.ChangePassword(account, dto);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        private Result HandleUpdateProfile(string account, EditProfileVm vm)
        {
            var service = new UserService();
            try
            {
                EditProfileDto dto = WebApiApplication._mapper.Map<EditProfileDto>(vm);
                service.UpdateProfile(dto);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
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
            int roleNumber = service.GetUserRole(account).Data as int? ?? 0; //取得角色數字
            System.Diagnostics.Debug.WriteLine($"Role Number: {roleNumber}");


            // 建立一張認證票
            var ticket =
                new FormsAuthenticationTicket(
                    1,                          //版本號
                    account,                    //
                    DateTime.Now,               //發行日
                    DateTime.Now.AddMinutes(30),    //到期日
                    false,                      //是否續存
                    roleNumber.ToString(),      // 在 UserData 中存入角色數字
                    "/"                         //cookie位置
                    );
            
            //將它加密
            var value = FormsAuthentication.Encrypt(ticket);



            // 解密並輸出調試信息
            var decryptedTicket = FormsAuthentication.Decrypt(value);
            System.Diagnostics.Debug.WriteLine($"Decrypted Role from Ticket: {decryptedTicket.UserData}");  // 確認UserData包含正確的角色數字


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
            //    service.AssignRole(dto.Id, dto.Roles);

            //    return Result.Success();
            //}

            //catch (Exception ex)
            //{
            //    return Result.Fail(ex.Message);
            //}


            try
            {
                
                RegisterDto dto = vm.ToDto();  // 將 ViewModel 轉換為 DTO
                service.Register(dto);         // 呼叫服務層進行註冊操作

                return Result.Success();
            }
            catch (DbEntityValidationException ex)  // 捕捉資料庫驗證錯誤
            {
                // 輸出每個驗證錯誤的詳細訊息
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                    }
                }
                return Result.Fail("Validation error occurred during registration.");
            }
            catch (DbUpdateException ex)  // 捕捉資料庫更新錯誤
            {
                // 輸出資料庫更新錯誤的詳細內部訊息
                System.Diagnostics.Debug.WriteLine($"DB Update Error: {ex.InnerException?.Message}");
                return Result.Fail("Database update error occurred during registration.");
            }
            catch (Exception ex)  // 捕捉其他所有錯誤
            {
                // 輸出一般錯誤的詳細內部訊息
                System.Diagnostics.Debug.WriteLine($"General Error: {ex.InnerException?.Message ?? ex.Message}");
                return Result.Fail("An error occurred during registration.");
            }


        }
    }
}
