using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Models
{
    public class MyAuthorizeAttribute:AuthorizeAttribute
    {
        //指名那些權限能存取action
        public string Functions { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //若目前是已登入狀態
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //如果global.asax忘了寫AuthenticateRequest事件 ,則這裡無法順利轉型
                // 所以這裡失敗,就去global.asax 查原因
                CustomPrincipal currentUser = filterContext.HttpContext.User as CustomPrincipal;

                //若functions沒值,表示action 上寫的只是[MyAuthorize]而不是[MyAuthorize(Function="1,2")]
                //表示只要有登入就能存取此action
                if (string.IsNullOrEmpty(Functions)) return;

                string[] allowFuntions = Functions.Split(',')
                                                  .Select(x => x.Trim().ToLower())
                                                  .ToArray();

                //判斷是否目前使用者有上述功能權限
                if (allowFuntions.Any(f => currentUser.IsInRole(f))) return;

                //若沒有權限,則導向到登入頁 以下兩種寫法擇一即可
                filterContext.Result = new RedirectResult("/Users/Login");
                return;

                //若沒權限,擇導向自訂的錯誤頁
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            Controller = "Users",
                            Action = "NoPermission",
                        }
                        )
                    );
            }
            base.OnAuthorization(filterContext);
        }
    }
}