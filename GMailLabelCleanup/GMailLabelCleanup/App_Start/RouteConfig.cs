using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GMailLabelCleanup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Don't embed Gmail's string based label ids into the path:
            //Labels/Edit?id=BLAH
            routes.MapRoute(
                name: "Labels",
                url: "Labels/{action}",
                defaults: new { controller = "Labels", action = "Index" }
            );

            //Filters/Edit?id=123
            routes.MapRoute(
                name: "Filters",
                url: "Filters/{action}",
                defaults: new { controller = "Filters", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(name: "signin-google", url: "signin-google", defaults: new { controller = "Account", action = "ExternalLoginCallback" });
        }
    }
}
