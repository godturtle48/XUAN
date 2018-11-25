using System.Web.Mvc;
using System.Web.Routing;

namespace UET_BTL_VERSION_1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "UET_BTL_VERSION_1.Areas.SignIn.Controllers" }
            );
        }
    }
}
