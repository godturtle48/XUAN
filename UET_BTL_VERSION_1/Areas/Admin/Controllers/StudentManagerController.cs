using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Authority;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.Admin.Controllers
{
    [AuthorizeBusiness]
    public class StudentManagerController : Controller
    {
        // Khởi tạo đối tượng DBcontext để thao tác với CSDL
        private UetSurveyDbContext db = new UetSurveyDbContext();

        // Hiển thị danh sách các sinh viên theo phân trang
        [HttpGet]
        public ActionResult Index(int? page)
        {
            // Số sinh viên trên một trang
            int pageSize = 20;
            // Số trang hiện tại
            int pageNumber = (page ?? 1);
            // Tổng số sinh viên
            ViewBag.sum = db.Students.Count();
            return View(db.Students.ToList().ToPagedList(pageNumber, pageSize));
        }

        // Trả về kết quả khi tìm kiếm 
        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            // Lấy từ khóa tìm kiếm từ thanh search
            string sTuKhoa = f["txtTimKiem"].ToString();
            // Truyền từ khóa tìm kiếm sang view hiển thị
            ViewBag.tukhoa = sTuKhoa;
            // Lấy danh sách các học sinh có chứa từ khóa tìm kiếm
            List<Student> listKQ = db.Students.Where(x => x.Name.Contains(sTuKhoa)).ToList();
            int pageSize = 200;
            int pageNumber = (page ?? 1);
            // Kiểm tra xem danh sách tìm kiếm có rỗng không
            if (listKQ.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sinh viên nào";
                return View(db.Students.ToList().ToPagedList(pageNumber, pageSize));
            }
            // Truyền thông báo ra view
            ViewBag.messageSearch = "Kết quả tìm kiếm với từ khóa : " + sTuKhoa;
            ViewBag.sum = listKQ.Count();
            return View(listKQ.ToPagedList(pageNumber, pageSize));
        }

        // Thêm mới sinh viên và trả về json cho client
        [HttpPost]
        public JsonResult Create(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            // Lấy username và kiếm tra xem đã tồn tại chưa
            string username = form["UserName"].ToString();
            if (db.Users.Any(x => x.UserName.Equals(username)))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Khởi tạo sinh viên, gán giá trị và lưu vào DB
                Student stu = new Student();
                stu.Name = form["Name"].ToString();
                stu.Course = form["Course"].ToString();
                stu.StudentCode = form["StudentCode"].ToString();
                stu.UserName = form["UserName"].ToString();
                stu.Email = form["Email"].ToString();
                stu.PassWord = form["PassWord"].ToString();
                db.Students.Add(stu);
                db.SaveChanges();
                int studentid = db.Students.Max(x => x.StudentID);
                // Khởi tạo user, gán trị sinh viên vừa lưu vào và lưu vào DB
                User user = new User()
                {
                    UserName = stu.UserName,
                    PassWord = stu.PassWord,
                    Position = "Student",
                    StudentID = studentid
                };
                db.Users.Add(user);
                db.SaveChanges();
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
        }

        // Chỉnh sửa sinh viên
        [HttpPost]
        public JsonResult Edit(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            // Lấy username từ form submit lên
            string username = form["UserName"].ToString();
            // Lấy id của sinh viên từ form
            int idStudent = int.Parse(form["idStudent"].ToString());
            // Kiểm tra xem sinh viên có bị trùng lặp không
            if (db.Users.Any(x => x.UserName.Equals(username) && x.StudentID != idStudent))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Cập nhật thông tin cho user và sinh viên theo các giá trị được submit từ form
                User user = db.Users.FirstOrDefault(x => x.StudentID == idStudent);
                user.UserName = username;
                user.PassWord = form["PassWord"].ToString();
                Student stu = db.Students.FirstOrDefault(x => x.StudentID == idStudent);
                stu.Name = form["Name"].ToString();
                stu.Course = form["Course"].ToString();
                stu.StudentCode = form["StudentCode"].ToString();
                stu.UserName = username;
                stu.Email = form["Email"].ToString();
                stu.PassWord = form["PassWord"].ToString();
                db.SaveChanges();
                return Json(new { status = 2 }, JsonRequestBehavior.AllowGet);
            }
        }

        // Xóa sinh viên theo id
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            Student student = db.Students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            return Json(student, JsonRequestBehavior.AllowGet);
        }

        // Xác nhận xóa
        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            // Tìm kiếm sinh viên theo mã
            Student student = db.Students.FirstOrDefault(x => x.StudentID == id);
            if (student == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Tìm kiếm user theo mã sinh viên
            User user = db.Users.FirstOrDefault(x => x.StudentID == id);
            // Lấy ra danh sách sinh viên chi tiết theo mã
            IEnumerable<StudentDetail> list2 = db.StudentDetails.Where(x => x.StudentID == id);
            foreach (var item in list2)
            {
                // Lấy ra các survey của sinh viên đó
                IEnumerable<Survey> list3 = db.Surveys.Where(x => x.StudentDetailID == item.StudentDetailID);
                try
                {
                    // Xóa toàn bộ survey của sinh viên đó
                    db.Surveys.RemoveRange(list3);
                }
                catch (Exception)
                {

                }
            }
            // Xóa toàn bộ sinh viên trong các lớp học phần
            db.StudentDetails.RemoveRange(list2);
            // Xóa user đại diện cho sinh viên đó
            db.Users.Remove(user);
            // Xóa sinh viên đó
            db.Students.Remove(student);
            db.SaveChanges();
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        // Impoert sinh viên từ excel từ fileUpload
        [HttpPost]
        public ActionResult ImportStudent(HttpPostedFileBase fileUpload)
        {
            ViewBag.message = "Import không thành công";
            int count = 0;
            try
            {
                // Thông báo tổng số sinh viên được import
                ViewBag.countStudent = 0;
                // Lấy file đã chọn để thao tác 
                var package = new ExcelPackage(fileUpload.InputStream);
                if (ImportData(out count, package))
                {
                    ViewBag.message = "Import thành công";
                }
            }
            catch (Exception)
            {
                return View();
            }
            // Thông báo tổng số sinh viên được import
            ViewBag.countStudent = count;
            return View();
        }

        // Import data 
        public bool ImportData(out int count, ExcelPackage package)
        {
            count = 0;
            var result = false;
            try
            {
                // Bắt đầu từ cột 1
                int startColumn = 1;
                // Bắt đầu từ hàng 2
                int startRow = 2;
                // Lấy sheet 1 của file excel
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                object data = null;
                // Khởi tạo đối tượng context để thao tác csdl
                UetSurveyDbContext db = new UetSurveyDbContext();

                do
                {
                    // Lấy các giá trị của từng hàng 
                    data = workSheet.Cells[startRow, startColumn].Value;
                    object userName = workSheet.Cells[startRow, startColumn + 1].Value;
                    object passWord = workSheet.Cells[startRow, startColumn + 2].Value;
                    object fullName = workSheet.Cells[startRow, startColumn + 3].Value;
                    object email = workSheet.Cells[startRow, startColumn + 4].Value;
                    object course = workSheet.Cells[startRow, startColumn + 5].Value;

                    // Kiểm tra xem đã cuối danh sách excel chưa
                    if (data != null)
                    {
                        // Truyền dữ liệu một sinh viên vào lưu vào DB
                        var isSuccess = SaveStudent(userName.ToString(), passWord.ToString(), fullName.ToString(), email.ToString(), course.ToString(), db);
                        if (isSuccess)
                        {
                            count++;
                            result = true;
                        }
                    }
                    startRow++;
                } while (data != null);
            }
            catch (Exception )
            {


            }
            return result;
        }

        // Lưu mỗi sinh viên vào DB
        public bool SaveStudent(string userName, string passWord, string fullName, string email, string course, UetSurveyDbContext db)
        {
            var result = false;
            try
            {
                // Kiểm tra xem sinh viên đã tồn tại chưa
                if (!db.Students.Any(x => x.UserName.Equals(userName)))
                {
                    // Khởi tạo sinh viên và gán dữ liệu rồi lưu vào db
                    var stu = new Student();
                    stu.UserName = userName;
                    stu.PassWord = passWord;
                    stu.StudentCode = userName;
                    stu.Name = fullName;
                    stu.Email = email;
                    stu.Course = course;
                    db.Students.Add(stu);
                    db.SaveChanges();
                    // Lấy Id sinh viên vừa tạo 
                    int studentid = db.Students.Max(x => x.StudentID);
                    // Khởi tạo user rồi gán giá trị sinh viên vừa lưu
                    User user = new User()
                    {
                        UserName = userName,
                        PassWord = passWord,
                        Position = "Student",
                        StudentID = studentid
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception )
            {

            }
            return result;

        }
    }
}