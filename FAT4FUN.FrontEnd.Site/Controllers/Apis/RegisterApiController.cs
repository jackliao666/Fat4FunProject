using FAT4FUN.FrontEnd.Site.Helpers;
using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;

namespace FAT4FUN.FrontEnd.Site.Controllers.Apis
{
    public class RegisterApiController : ApiController
    {
        [HttpPost]
        [Route("api/MembersApi/Register")]
        public async Task<HttpResponseMessage> Register([FromBody] RegisterVm vm)

        {
            // 1. 檢查請求資料是否為 null，確保用戶有提供註冊資訊
            if (vm == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data."); // 返回 400 Bad Request
            }

            // 2. 檢查必填欄位是否有填寫，確保用戶填寫必要的註冊資料
            if (string.IsNullOrEmpty(vm.Name) || string.IsNullOrEmpty(vm.Account) ||
                string.IsNullOrEmpty(vm.Email) || string.IsNullOrEmpty(vm.Password))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "All fields are required."); // 返回 400 Bad Request
            }

            // 3. 使用資料庫上下文檢查是否已存在相同帳號
            using (var db = new AppDbContext()) // 創建資料庫上下文的實例
            {
                // 查詢資料庫中是否已有相同帳號的用戶
                var existingUser = await db.Users
                    .FirstOrDefaultAsync(u => u.Account == vm.Account);

                // 4. 如果用戶已存在，返回 409 Conflict
                if (existingUser != null)
                {
                    //return Request.CreateResponse(HttpStatusCode.InternalServerError, "註冊失敗: " + ex.Message);
                    return Request.CreateResponse(HttpStatusCode.Conflict, "Account already exists."); // 返回 409 Conflict
                }

                // 5. 創建新用戶實例並填入資料
                var newUser = new User
                {
                    Name = vm.Name, // 設定用戶名稱
                    Gender = vm.Gender,
                    Account = vm.Account, // 設定用戶帳號
                    Password = EncryptPassword(vm.Password), // 對密碼進行雜湊處理
                    //Password = vm.Password,
                    Phone = vm.Phone, // 設定用戶電話
                    Email = vm.Email, // 設定用戶郵箱        
                    Address = vm.Address,
                    Status = false,
                    CreateDate = DateTime.Now, // 設定創建日期為當前時間
                    ModifyDate = DateTime.Now,
                    IsConfirmed = false // 新用戶預設為未確認狀態
                };
                

                // 生成確認碼
                string confirmCode = Guid.NewGuid().ToString("N");

                // 將確認碼存入資料庫
                newUser.ConfirmCode = confirmCode;

                // 6. 將新用戶添加到資料庫
                db.Users.Add(newUser); // 將新用戶添加到 Users 集合中
                await db.SaveChangesAsync(); // 保存變更到資料庫

                var newRole = new Role
                {
                    UserId = newUser.Id, // 使用剛剛創建用戶的 ID
                    Role1 = 5 // 這裡可以更具需求設定角色名稱
                };

                db.Roles.Add(newRole);
                await db.SaveChangesAsync();

                // 假設您的應用程式運行在 localhost，並且使用 https
                string baseUrl = "https://localhost:44342"; // 根據您的實際域名或地址進行調整
                string activationLink = $"{baseUrl}/html/activation.html?userId={newUser.Id}&confirmCode={confirmCode}";

                // 構建郵件內容
                string subject = "註冊成功!";
                string body = $"歡迎加入會員！請點擊以下連結以確認您的帳戶：<br/>" +
                              $"<a href='{activationLink}'>請點擊這裡</a>";

                // 發送郵件
                EmailHelper emailHelper = new EmailHelper();
                //emailHelper.SendFromGmail("christine10150193@gmail.com", newUser.Account, subject, body);
                emailHelper.SendFromGmail("pianoprince35@gmail.com", newUser.Email, subject, body);



                return Request.CreateResponse(HttpStatusCode.OK, "註冊成功");
            }
        }

        // 啟動帳號
        [HttpGet]
        [Route("api/MemberApi/ActiveRegister")]
        public async Task<HttpResponseMessage> ActiveRegister(int userId, string confirmCode)
        {
            // 檢查參數是否有效
            if (string.IsNullOrEmpty(confirmCode))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "無效的確認連結");
            }

            using (var db = new AppDbContext())
            {
                // 根據 memberId 查找會員
                var user = await db.Users.FirstOrDefaultAsync(m => m.Id == userId);
                if (user == null || user.ConfirmCode != confirmCode)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "無效的確認連結");
                }

                // 更新會員的 IsConfirmed 欄位
                user.IsConfirmed = true;
                user.ConfirmCode = null; // 清除確認碼
                await db.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, "帳號已成功啟用");
            }
        }
        private string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
