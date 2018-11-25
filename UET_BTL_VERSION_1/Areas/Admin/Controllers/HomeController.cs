using System.Linq;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Authority;

namespace UET_BTL_VERSION_1.Areas.Admin.Controllers
{
    [AuthorizeBusiness]
    public class HomeController : Controller
    {
        // Khởi tạo đối tượng DBcontext để thao tác với CSDL
        private UetSurveyDbContext db = new UetSurveyDbContext();

        // Hiển thị trang chủ của index gồm số người online, số sinh viên, giảng viên ...
        public ActionResult Index()
        {
            // Kiểm tra session đã tồn tại chưa 
            if (Session["user"] != null)
            {
                // Gán các tham số tống số sinh viên, môn học, đánh giá, người online để truyền sang view
                ViewBag.SumStudents = db.Students.ToList().Count();
                ViewBag.SumSubjects = db.Subjects.ToList().Count();
                ViewBag.SumSurveys = db.Surveys.GroupBy(x => x.StudentDetailID).Count();
                ViewBag.SumUserOnline = HttpContext.Application["Online"].ToString();

                return View();
            }
            // Chuyển hướng đến trang login
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

    }
}