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
            //�p�G�|���n�J,���B�z
            if (!Request.IsAuthenticated) return;

            //���oFormsIdentity
            var identity = (FormsIdentity)User.Identity;

            //�M����o�{�Ҳ�
            FormsAuthenticationTicket ticket = identity.Ticket;

            //���o�������ϥΪ̸�T
            string functions = ticket.UserData;

            //�إߤ@�Ӧۭq���ϥΪ̪���
            IPrincipal principal = new CustomPrincipal(identity, functions);

            //�⴫���ڭ̦ۤv���ϥΪ̪���
            Context.User = principal;

        }


        private string GetRoleNameFromNumber(int roleNumber)
        {
            switch (roleNumber)
            {
                case 0: return "Admin";
                case 1: return "Employee";
                case 2: return "Designer";
                default: return "Unknown";
            }
        }
    }
}
