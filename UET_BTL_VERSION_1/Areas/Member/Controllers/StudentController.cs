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
    public class StudentController : Controller
    {
        // Khởi tạo đối tượng DBcontext đê thao tác với csdl
        private UetSurveyDbContext db = new UetSurveyDbContext();

        // Hiển thị thông tin thống kê của sinh viên
        public ActionResult Index()
        {
            // Lấy user từ session
            User user = Session["user"] as User;
            // Kiểm tra user có tồn tại không
            if (user != null)
            {
                // Lấy ra sinh viên theo ID
                Student stu = db.Students.FirstOrDefault(s => s.StudentID == user.StudentID);
                // Lấy tổng môn học của sinh viên và truyền sang view
                ViewBag.SumSubject = stu.StudentDetail.Count();
                // Lấy  danh sách các chi tiết sinh viên - học phần
                List<int> listID = stu.StudentDetail.Select(y => y.StudentDetailID).ToList();
                // Lấy tổng số môn học đã đánh giá
                ViewBag.SumSubjectSurvey = db.Surveys.Where(x => listID.Any(y => y == x.StudentDetailID)).GroupBy(x => x.StudentDetailID).Count();
                // Lấy tổng số người online
                ViewBag.SumUserOnline = HttpContext.Application["Online"].ToString();
                return View();
            }
            return RedirectToAction("Login","Home",new { area = "SignIn"});
        }

        // Hiển thị thông tin của sinh viên
        public ActionResult ShowInforStudent()
        {
            // Lấy user từ Session
            User user = Session["user"] as User;
            // Kiểm tra user có tồn tại không
            if (user != null)
            {
                // Lấy sinh viên theo mã ID
                Student student = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                return View(student);
            }
            // Chuyển hướng đến trang login
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        // Submit form khi sinh viên đổi mật khẩu
        [HttpPost]
        public ActionResult ShowInforStudent(FormCollection f)
        {
            // Lấy user từ session
            User user = Session["user"] as User;
            // Lấy mật khẩu cũ
            string pass = f["passOld"].ToString();
            // Kiểm tra user có tồn tại không
            if (user != null)
            {
                // Lấy sinh viên theo mã 
                Student stu = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                // Kiểm tra mật khẩu cũ có trùng không, mật khẩu mới có khớp không
                if ((pass != user.PassWord) || (f["passNew1"].ToString().Trim() != f["passNew2"].ToString().Trim()))
                {
                    ViewBag.Message = "Mật khẩu cũ không đúng hoặc hai mật khẩu mới không khớp nhau!";
                    return View(stu);
                }
                // Lấy user theo id của sinh viên và cập nhật giá trị cho các thuộc tính
                User u = db.Users.FirstOrDefault(x => x.UserName == user.UserName);
                u.PassWord = f["passNew1"].ToString();
                stu.PassWord = f["passNew1"].ToString();
                db.SaveChanges();
                ViewBag.Message = "Đã đổi mật khẩu thành công";
                return View(stu);
            }
            // Chuyển hướng đến trang login
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        // Hiển thị các môn học của sinh viên
        public ActionResult ShowListSubject()
        {
            // Lấy user từ session
            User user = Session["user"] as User;
            // Kiểm tra user có tồn tại không
            if (user != null)
            {
                // Lấy ra các thông tin chi tiết sinh viên - môn học
                IEnumerable<StudentDetail> listStudent = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID).StudentDetail.ToList();
                return View(listStudent);
            }
            // Chuyển hướng sang trang login
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        // Đánh giá môn học 
        public ActionResult SurveySubject(int? id, int? stID)
        {
            if (id == null || stID == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy thông tin chi tiết môn học - sinh viên
            StudentDetail student_Detail = db.StudentDetails.First(x => x.SubjectID == id && x.StudentID == stID);
            // Kiểm tra sinh viên đã đánh giá chưa
            if (db.Surveys.Any(x => x.StudentDetailID == student_Detail.StudentDetailID))
            {
                ViewBag.Message = "Bạn đã đánh giá môn học này";
                return View(student_Detail);
            }
            // Lấy tổng số tiêu chí đánh giá
            ViewBag.Count = db.ContentSurveys.ToList().Count();
            // Lấy danh sách các tiêu chí đánh giá
            ViewBag.ContentSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            return View(student_Detail);
        }

        // Submit đánh giá môn học từ form
        [HttpPost]
        public ActionResult SurveySubject(FormCollection form)
        {
            // Lấy tổng số tiêu chí đánh giá
            ViewBag.Count = db.ContentSurveys.ToList().Count();
            // Lấy các tiêu chí đánh giá
            ViewBag.ContentSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            // Lấy id từ form
            int k = int.Parse(form["id"]);
            int stDetailID = int.Parse(form["studentDetailID"]);
            StudentDetail student_Detail = db.StudentDetails.First(x => x.SubjectID == k);
            // Kiểm tra đã đánh giá đủ các tiêu chí chưa
            if (form.Count < db.ContentSurveys.ToList().Count() + 3)
            {
                ViewBag.Message2 = "Bạn cần đánh giá đủ các tiêu chí";
                return View(student_Detail);
            }
            int i = 0;
            foreach (var item in db.ContentSurveys)
            {
                // Khởi tạo Survey và gán giá trị cho các thuộc tính
                Survey survey = new Survey();
                survey.StudentDetailID = stDetailID;
                survey.ContentSurveyID = item.ContentSurveyID;
                survey.Point = int.Parse(form[i++]);
                db.Surveys.Add(survey);
            }
            // Lấy thông tin chi tiết sinh viên - môn học
            StudentDetail stDetail = db.StudentDetails.FirstOrDefault(x => x.StudentDetailID == stDetailID);
            stDetail.NoteSurvey = form["note"].ToString();
            db.SaveChanges();
            return RedirectToAction("ShowListSubject");
        }
    }
}