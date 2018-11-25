using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Entities;
using System.Net.Mail;

namespace UET_BTL_VERSION_1.Areas.SignIn.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo đối tượng DBcontext để thao tác csdl
        private UetSurveyDbContext db = new UetSurveyDbContext();
       
        // Hiển thị trang đăng nhập
        [HttpGet]
        public ActionResult Login()
        {
            // Lấy user từ session
            User user = Session["user"] as User;

            if (user != null)
            {
                // Kiểm tra loại user để chuyển hướng
                if (user.Position.Equals("Teacher"))
                {
                    // Lấy ra giảng viên theo id
                    Teacher teacher = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID);
                    // Lưu tên user vào session
                    Session["fullName"] = teacher.Name.ToUpper();
                    // CHuyển hướng đến trang chủ cảu giảng viên
                    return RedirectToAction("Index", "Teacher", new { area = "Member" });
                }
                else if (user.Position.Equals("Student"))
                {
                    // Lấy ra sinh viên theo id
                    Student stu = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                    // Lưu tên sinh viên vào session
                    Session["fullName"] = stu.Name.ToUpper();
                    // Chuyển hướng đến trang chủ của sinh viên
                    return RedirectToAction("Index", "Student", new { area = "Member" });
                }
                else
                {
                    // Lưu tên admin vào session
                    Session["fullName"] = "Admin";
                    // Chuyển hướng đến trang chủ của admin
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            return View("~/Areas/SignIn/Views/Home/Login.cshtml");
        }

        // Submit đăng nhập từ form
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            // Lấy username từ form
            string username = form["username"].ToString().Trim();
            // Lấy mật khẩu từ form
            string password = form["password"].ToString().Trim();
            // Lấy user có username và password trùng với form submit
            User user = db.Users.FirstOrDefault(x => x.UserName.Equals(username) && x.PassWord.Equals(password));
            // Kiểm tra xem user có tồn tại không
            if (user != null)
            {
                // Gán user vào sesssion
                Session["user"] = user;
               
                // Kiểm tra loại user để chuyển hướng
                if (user.Position.Equals("Teacher"))
                {
                    // Lấy ra giảng viên theo id
                    Teacher teacher = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID);
                    // Lưu tên user vào session
                    Session["fullName"] = teacher.Name.ToUpper();
                    // CHuyển hướng đến trang chủ cảu giảng viên
                    return RedirectToAction("Index", "Teacher", new { area = "Member" });
                }
                else if (user.Position.Equals("Student"))
                {
                    // Lấy ra sinh viên theo id
                    Student stu = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                    // Lưu tên sinh viên vào session
                    Session["fullName"] = stu.Name.ToUpper();
                    // Chuyển hướng đến trang chủ của sinh viên
                    return RedirectToAction("Index", "Student",new { area = "Member" });
                }
                else
                {
                    // Lưu tên admin vào session
                    Session["fullName"] = "Admin";
                    // Chuyển hướng đến trang chủ của admin
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

            }
            ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            return View();
        }

        // Đăng xuất tài khoản người dùng
        public ActionResult Logout()
        {
            // Hủy session lưu user và tên
            Session.Remove("user");
            Session.Remove("fullname");
            // Chuyển hướng đến trang login
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        // Lấy ra menu tương ứng với từng layout của user
        public PartialViewResult MenuPartial()
        {
            // Lấy user từ session
            User user = Session["user"] as User;
            // Kiểm tra user có tồn tại không
            if (user != null)
            {
                ViewBag.position = user.Position.ToString();
            }
            return PartialView();
        }

        // Hiển thị thông báo lỗi khi không được quyền truy cập
        public ActionResult Authorize()
        {
            return View();
        }

        // Hiển thị khi không tìm thấy trang
        public ActionResult NotFoundWebsite()
        {
            return View();
        }

        // Lấy lại mật khẩu
        [HttpPost]
        public JsonResult GetPassWord(FormCollection form)
        {
            // Lấy username và email từ form submit
            string username = form["Username"].ToString().Trim();
            string email = form["Email"].ToString().Trim();

            // Lấy ra sinh viên có tên và email thỏa mãn
            Student student = db.Students.FirstOrDefault(s => s.UserName.Equals(username) && s.Email.Equals(email));
            if (student != null)
            {
                string emailAddress = student.Email;
                string pass = student.PassWord;
                string content = "<h1>Thông tin tài khoản là : </h1></br> ";
                content += "<h1> Tên đăng nhập:  " + username + "</h1></br> ";
                content += "<h1> Mật khẩu: " + pass + "</h1></br> ";
                GuiEmail("Thông tin tài khoản", emailAddress , "tienxuantt@gmail.com", "Xuan06061998@", content);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            // Lấy ra giảng viên có tên và email thỏa mãn
            Teacher teacher = db.Teachers.FirstOrDefault(s => s.UserName.Equals(username) && s.Email.Equals(email));
            if (teacher != null)
            {
                string emailAddress = teacher.Email;
                string pass = teacher.PassWord;
                string content = "<h1>Thông tin tài khoản là : </h1></br>";
                content += "<h1>Tên đăng nhập: " + username + "</h1></br>";
                content += "<h1>Mật khẩu: " + pass + "</h1></br>";
                GuiEmail("Thông tin tài khoản", emailAddress, "tienxuantt@gmail.com", "Xuan06061998@", content);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
        }

        // Gửi email
        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chửi gửi
            mail.Subject = Title; // tiêu đề gửi
            mail.Body = Content; // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587; //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true; //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail); //Gửi mail đi
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
