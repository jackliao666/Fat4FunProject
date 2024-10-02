using FAT4FUN.FrontEnd.Site.Models.EFModels;
using FAT4FUN.FrontEnd.Site.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace FAT4FUN.FrontEnd.Site.Controllers.Apis
{
    public class LoginApiController : ApiController
    {
        private const string AuthCookieName = "auth_token";

        [HttpPost]
        [Route("api/MemberApi/Login")]
        public HttpResponseMessage Login(LoginVm vm)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Account == vm.Account);
                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號或密碼錯誤");

                if ((bool)!user.IsConfirmed)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "尚未開通，請至信箱點擊連結");
                }

                if (!VerifyPassword(vm.Password, user.Password))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號或密碼錯誤");

                var userData = new
                {
                    account = user.Account,
                    name = user.Name,
                    phone = user.Phone,
                    registrationDate = user.CreateDate,
                    id = user.Id,
                    address = user.Address,
                    email = user.Email,
                    gender = user.Gender
                };

                var response = Request.CreateResponse(HttpStatusCode.OK, userData);
                var cookie = new CookieHeaderValue(AuthCookieName, GenerateToken(user.Account))
                {
                    HttpOnly = false,
                    Expires = DateTimeOffset.Now.AddHours(2),
                    Domain = Request.RequestUri.Host,
                    Path = "/"
                };

                response.Headers.AddCookies(new[] { cookie });
                return response;
            }
        }

        [HttpPost]
        [Route("api/MemberApi/Logout")]
        public HttpResponseMessage Logout()
        {
            var cookie = new CookieHeaderValue(AuthCookieName, "")
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                Path = "/"
            };

            var response = Request.CreateResponse(HttpStatusCode.OK, "成功登出");
            response.Headers.AddCookies(new[] { cookie });
            return response;
        }

        private bool VerifyPassword(string password, string encryptedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(inputBytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString == encryptedPassword;
            }
        }

        private string GenerateToken(string account)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{account}:{DateTime.UtcNow}"));
            return token;
        }

        private bool IsAuthenticated()
        {
            var cookieHeader = Request.Headers.GetCookies(AuthCookieName).FirstOrDefault();
            return cookieHeader != null && cookieHeader[AuthCookieName] != null;
        }

        [HttpGet]
        [Route("api/MemberApi/GetMemberData")]
        public HttpResponseMessage GetMemberData(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號不能為空");
            }

            if (!IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "User is not authenticated.");
            }

            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Account == account);
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Member not found");
                }

                var userData = new
                {
                    account = user.Account,
                    name = user.Name,
                    phone = user.Phone,
                    registrationDate = user.CreateDate,
                };

                return Request.CreateResponse(HttpStatusCode.OK, userData);
            }
        }

        //[HttpPut]
        //[Route("api/MemberApi/UpdateMemberData")]
        //public HttpResponseMessage UpdateMemberData(MemberUpdateVm memberUpdate)
        //{
        //    if (string.IsNullOrWhiteSpace(memberUpdate.Account))
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號不能為空");
        //    }

        //    if (!IsAuthenticated())
        //    {
        //        return Request.CreateResponse(HttpStatusCode.Unauthorized, "User is not authenticated.");
        //    }

        //    using (var db = new AppDbContext())
        //    {
        //        var member = db.Members.FirstOrDefault(x => x.Account == memberUpdate.OriginalAccount);
        //        if (member == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "找不到該會員");
        //        }



        //        if (memberUpdate.Account != member.Account &&
        //            db.Members.Any(x => x.Account == memberUpdate.Account && x.Account != member.Account))
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, "新帳號已被其他用戶使用，請選擇其他帳號。");
        //        }
        //        member.Name = memberUpdate.Name;
        //        member.Phone = memberUpdate.Phone;



        //        if (memberUpdate.Account != member.Account)
        //        {
        //            member.Account = memberUpdate.Account;
        //        }

        //        db.SaveChanges();


        //        var updatedMemberData = new
        //        {
        //            account = member.Account,
        //            name = member.Name,
        //            phone = member.Phone,
        //            profilePhoto = member.ProfilePhoto,
        //            registrationDate = member.RegistrationDate,
        //            points = member.Points
        //        };

        //        return Request.CreateResponse(HttpStatusCode.OK, updatedMemberData);
        //    }
        //}


        [HttpGet]
        [Route("api/Memberapi/Members")]
        public IHttpActionResult GetAllMembers(int memberId)
        {
            var db = new AppDbContext();

            var data = db.Users
                .Where(m => m.Id == memberId)
                .Select(x => new MemberVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Account = x.Account, // 這裡用 Account 對應 Email
                    EncryptedPassword = x.Password, // 使用加密的密碼
                    Phone = x.Phone,
                    RegistrationDate = x.CreateDate,
                })
                .FirstOrDefault(); // 根據 MemberId 返回單一會員資料

            if (data == null)
            {
                return NotFound(); // 如果找不到該會員，返回 404 錯誤
            }

            return Ok(data); // 返回會員資料
        }
    }
}
