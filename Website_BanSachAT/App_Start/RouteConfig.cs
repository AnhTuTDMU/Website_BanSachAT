using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Website_BanSachAT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

              routes.MapRoute(
             name: "Trang Chủ",
             url: "",
             defaults: new { controller = "Book", action = "Index", id = UrlParameter.Optional }
             , namespaces: new[] { "Website_BanSachAT.Controllers" }

         );
            routes.MapRoute(
                name: "Trang tin",
                url: "{metatitle}",
                defaults: new { controller = "Book", action = "TrangTin", metatitle = UrlParameter.Optional },
                namespaces: new string[] { "Website_BanSachAT.Controllers" }
           );
            routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
             , namespaces: new[] { "Website_BanSachAT.Controllers" }

         );
        }
    }
}
