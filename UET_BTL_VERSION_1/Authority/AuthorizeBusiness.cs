using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model.Entities;

namespace UET_BTL.Model.Authority
{
    public class AuthorizeBusiness : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            // Lấy user hiện tại từ session
            User user = HttpContext.Current.Session["user"] as User;
            // Kiểm tra xem user có tồn tại không
            if (user == null)
            {
                // Chuyển hướng đến trang đăng nhập
                filterContext.Result = new RedirectResult("/dang-nhap");
                return;
            }
            // Lấy token của trang hiện tại
            var dataTokens = HttpContext.Current.Request.RequestContext.RouteData.DataTokens;
            // Lấy dữ liệu router
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            // Lấy action của router
            string action = routeValues.ContainsKey("action") ? (string)routeValues["action"] : string.Empty;
            // Lấy controller của router
            string controller = routeValues.ContainsKey("controller") ? (string)routeValues["controller"] : string.Empty;
            // Lấy area của router
            string area = dataTokens.ContainsKey("area") ? (string)dataTokens["area"] : string.Empty;
            // Khởi tạo DBcontext để thao tác csdl
            UetSurveyDbContext db = new UetSurveyDbContext();
            // Kiểm tra các action, controller, area có hợp lệ không
            bool check = db.Roles.Any(s => s.Position == user.Position && s.Area == area && s.Controller == controller && s.Action == action);
            // Nếu không hợp lệ thì điều hướng tới trang error
            if (!check)
            {
                filterContext.Result = new RedirectResult("/error");
                return;
            }
        }

    }
}
