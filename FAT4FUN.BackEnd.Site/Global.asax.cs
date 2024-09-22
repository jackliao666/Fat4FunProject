using AutoMapper;
using FAT4FUN.BackEnd.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FAT4FUN.BackEnd.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IMapper _mapper;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            //AutoMapper 配置
            var config = new MapperConfiguration(cfg =>
            {
                //使用 MappingProfile 來設定對應關係
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();


        }

    }
}
