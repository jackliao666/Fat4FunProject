using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Models
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        //指名那些權限能存取action
        public string Functions { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // 若目前是已登入狀態
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // 確保 CustomPrincipal 正確轉型
                CustomPrincipal currentUser = filterContext.HttpContext.User as CustomPrincipal;

                // 若找不到 CustomPrincipal，則導向到登入頁
                if (currentUser == null)
                {
                    filterContext.Result = new RedirectResult("/Users/Login");
                    return;
                }

                // 若未設置 Functions，則允許所有登入者訪問
                if (string.IsNullOrEmpty(Functions))
                {
                    return;
                }

                // 分析允許的功能
                string[] allowedFunctions = Functions.Split(',')
                                                      .Select(x => x.Trim().ToLower())
                                                      .ToArray();
                

                // 判斷目前使用者是否有上述功能權限
                if (!allowedFunctions.Any(f => currentUser.IsInRole(f)))
                {
                    // 若沒有權限，則導向到 NoPermission 頁面
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(
                            new { controller = "Users", action = "NoPermission" }
                        )
                    );
                    
                }
            }
            else
            {
                // 若未登入，則導向到登入頁
                filterContext.Result = new RedirectResult("/Users/Login");
            }
        }
    }
}