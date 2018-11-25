using System.Web.Mvc;

namespace UET_BTL_VERSION_1.Areas.SignIn
{
    public class SignInAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SignIn";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DangNhap",
                "dang-nhap",
                new {controller = "Home", action = "Login", id = UrlParameter.Optional }
            );
            context.MapRoute(
               "error",
               "error",
               new { controller = "Home", action = "Authorize", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "notfound",
               "notfound",
               new { controller = "Home", action = "NotFoundWebsite", id = UrlParameter.Optional }
           );
            context.MapRoute(
                "SignIn_default",
                "SignIn/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}