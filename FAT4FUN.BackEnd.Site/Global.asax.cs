using AutoMapper;
using FAT4FUN.BackEnd.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace FAT4FUN.BackEnd.Site
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IMapper _mapper;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);




            var config = new MapperConfiguration(cfg =>
            {
                //使用 MappingProfile 來設定對應關係
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
        }


        protected void Application_AuthenticateRequest(object sander, EventArgs e)
        {
            ////如果尚未登入,不處理
            //if (!Request.IsAuthenticated) return;

            ////取得FormsIdentity
            //var identity = (FormsIdentity)User.Identity;

            ////然後取得認證票
            //FormsAuthenticationTicket ticket = identity.Ticket;

            ////取得票中的使用者資訊
            //string functions = ticket.UserData;

            ////建立一個自訂的使用者物件
            //IPrincipal principal = new CustomPrincipal(identity, functions);

            ////抽換成我們自己的使用者物件
            //Context.User = principal;

            //if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    // 取得身份票
            //    FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;

            //    // 從 UserData 中解析角色數字
            //    string[] roles = ticket.UserData.Split(',');

            //    // 將當前使用者轉換為 CustomPrincipal
            //    HttpContext.Current.User = new CustomPrincipal(HttpContext.Current.User.Identity, roles);
            //}

            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                // 解密認證票
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null && !string.IsNullOrEmpty(authTicket.UserData))
                {
                    // 解析 UserData（這裡是角色數字）
                    string[] roles = authTicket.UserData.Split(',');

                    // 使用解密的資料創建 CustomPrincipal
                    CustomPrincipal newUser = new CustomPrincipal(new GenericIdentity(authTicket.Name), roles);

                    // 設置 HttpContext 的 User 物件
                    HttpContext.Current.User = newUser;
                }
            }



        }


        private string GetRoleNameFromNumber(int roleNumber)
        {
            switch (roleNumber)
            {
                case 0: return "Admin";
                case 1: return "Manager";
                case 2: return "Designer";
                case 3: return "Sales";
                case 4: return "Human Resources";
                case 5: return "Members";
                default: return "Unknown";
            }
        }
    }
}
