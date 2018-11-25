using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Authority;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.Member.Controllers
{
    [AuthorizeBusiness]
    public class TeacherController : Controller
    {
        // Khởi tạo đối tượng DBcontext để thao tác csdl
        private UetSurveyDbContext db = new UetSurveyDbContext();

        // Hiển thị các thông tin thống kế
        public ActionResult Index()
        {
                // Lấy user từ session
                User user = Session["user"] as User;
                // Kiểm tra user có tồn tại không
                if (user != null)
                {
                    // Lấy tổng sinh viên  giảng viên đang dạy
                    ViewBag.SumStudents = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID).StudentDetail.Count();
                    // Lấy tổng số học phần giảng viên đang dạy
                    ViewBag.SumSubjects = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID).StudentDetail.GroupBy(x => x.SubjectID).Count();
                    // Lấy tổng số người đang online
                    ViewBag.SumUserOnline = HttpContext.Application["Online"].ToString();
                    return View();
                }
                // Chuyển hướng đến trang login
                return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        // Hiển thị thông tin của giảng viên
        public ActionResult ShowInforTeacher()
        {
            // Lấy user từ session
            User user = Session["user"] as User;
            // Kiểm tra user có tồn tại không
            if (user != null)
            {
                // Lấy giảng viên theo mã id
                Teacher teacher = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID);
                return View(teacher);
            }
            // Chuyển hướng đến trang login
            return RedirectToAction("Login","Home",new { area = "SignIn" });
        }

        // Submit form khi giảng viên đổi mật khẩu
        [HttpPost]
        public ActionResult ShowInforTeacher(FormCollection f)
        {
            // Lấy user từ session
            User user = Session["user"] as User;
            // Lấy mật khẩu cũ từ form submit
            string pass = f["passOld"].ToString();
            // Kiểm tra user có tồn tại không
            if (user != null)
            {
                // Lấy giảng viên theo ID
                Teacher teacher = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID);
                // Kiểm tra mật khẩu cũ có đúng không, mật khẩu mới có khớp nhau không
                if ((pass != user.PassWord) || (f["passNew1"].ToString().Trim() != f["passNew2"].ToString().Trim()))
                {
                    ViewBag.Message = "Mật khẩu cũ không đúng hoặc hai mật khẩu mới không khớp nhau!";
                    return View(teacher);
                }
                // Lấy user của giảng viên và cập nhật các giá trị của thuộc tính
                User u = db.Users.FirstOrDefault(x => x.UserName == user.UserName);
                u.PassWord = f["passNew1"].ToString();
                teacher.PassWord = f["passNew1"].ToString();
                db.SaveChanges();
                ViewBag.Message = "Đã đổi mật khẩu thành công";
                return View(teacher);
            }
            // Chuyển hướng đến tran login
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        // Hiển thị danh sách các học phần
        public ActionResult ShowListSubject()
        {
            // Lấy user tư session
            User user = Session["user"] as User;
            // Lấy danh sách các sinh viên - giảng viên
            IEnumerable<StudentDetail> listStudent = db.StudentDetails.Where(x => x.TeacherID == user.TeacherID)
                .GroupBy(x => x.SubjectID).Select(x => x.FirstOrDefault());

            return View(listStudent);
        }

        // Hiển thị danh sách cả lớp sinh viên của một học phần
        public ActionResult ShowClass(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra danh sách sinh viên của học phần đó
            IEnumerable<StudentDetail> listStudent = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.ToList();
            return View(listStudent);
        }

        // Hiển thị kết quả đánh giá học phần của môn học đó
        public ActionResult ShowResultSurvey(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra danh sách id của sinh viên - học phần
            List<int> lis = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.Select(x => x.StudentDetailID).ToList();
            // Lấy ra một đối tượng sinh viên - học phần
            StudentDetail student_Detail = db.StudentDetails.First(x => x.SubjectID == id);
            // Lấy ra tổng số đánh giá học phần 
            ViewBag.hasSurvey = db.Surveys.Where(x => lis.Any(k => k == x.StudentDetailID)).ToList().Count();
            // Lấy ra tổng số sinh viên của một học phần
            ViewBag.SumStudent = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.ToList().Count();
            // Kiểm tra xem đã  có sinh viên nào đánh giá chưa
            if (ViewBag.hasSurvey == 0)
            {
                return View(student_Detail);
            }
            // Lấy ra danh sách điểm trung bình theo cả lớp của các tiêu chí đánh giá
            ViewBag.ListPointAver = db.Surveys
                .Where(x => lis.Any(k => k == x.StudentDetailID))
                .GroupBy(x => x.ContentSurveyID)
                .Select(x => x.Average(y => y.Point)).ToList();
            // Lấy ra các tiêu chí đánh giá
            ViewBag.NameSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            // Lấy ra tổng số tiêu chí đánh giá
            ViewBag.CountSurvey = db.ContentSurveys.ToList().Count();

            return View(student_Detail);
        }

    }
}