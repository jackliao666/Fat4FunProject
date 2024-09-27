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
                //�ϥ� MappingProfile �ӳ]�w�������Y
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
        }


        protected void Application_AuthenticateRequest(object sander, EventArgs e)
        {
            ////�p�G�|���n�J,���B�z
            //if (!Request.IsAuthenticated) return;

            ////���oFormsIdentity
            //var identity = (FormsIdentity)User.Identity;

            ////�M����o�{�Ҳ�
            //FormsAuthenticationTicket ticket = identity.Ticket;

            ////���o�������ϥΪ̸�T
            //string functions = ticket.UserData;

            ////�إߤ@�Ӧۭq���ϥΪ̪���
            //IPrincipal principal = new CustomPrincipal(identity, functions);

            ////�⴫���ڭ̦ۤv���ϥΪ̪���
            //Context.User = principal;

            //if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    // ���o������
            //    FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;

            //    // �q UserData ���ѪR����Ʀr
            //    string[] roles = ticket.UserData.Split(',');

            //    // �N��e�ϥΪ��ഫ�� CustomPrincipal
            //    HttpContext.Current.User = new CustomPrincipal(HttpContext.Current.User.Identity, roles);
            //}

            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                // �ѱK�{�Ҳ�
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null && !string.IsNullOrEmpty(authTicket.UserData))
                {
                    // �ѪR UserData�]�o�̬O����Ʀr�^
                    string[] roles = authTicket.UserData.Split(',');

                    // �ϥθѱK����ƳЫ� CustomPrincipal
                    CustomPrincipal newUser = new CustomPrincipal(new GenericIdentity(authTicket.Name), roles);

                    // �]�m HttpContext �� User ����
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
