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
    public class TeacherManagerController : Controller
    {
        // Khởi tạo đối tượng DBcontext để thao tác csdl
        private UetSurveyDbContext db = new UetSurveyDbContext();

        // Hiển thị các giảng viên theo phân trang
        public ActionResult Index(int? page)
        {
            // số giảng viên trên một trang
            int pageSize = 15;
            // Số trang hiện tại
            int pageNumber = (page ?? 1);
            // Tổng số giảng viên
            ViewBag.sum = db.Teachers.Count();
            // Truyền danh sách ra view
            return View(db.Teachers.ToList().ToPagedList(pageNumber, pageSize));
        }

        // Hiển thị danh sách giảng viên theo tìm kiếm
        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            // Lấy từ khóa tìm kiếm 
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.tukhoa = sTuKhoa;
            // Lấy ra các giảng viên có tên chứa từ khóa tìm kiếm
            List<Teacher> listKQ = db.Teachers.Where(x => x.Name.Contains(sTuKhoa)).ToList();
            int pageSize = 200;
            int pageNumber = (page ?? 1);
            // Kiểm tra kết quả tìm kiếm
            if (listKQ.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy giang viên nào";
                return View(db.Teachers.ToList().ToPagedList(pageNumber, pageSize));
            }
            // Truyền thông báo sang view
            ViewBag.messageSearch = "Kết quả tìm kiếm với từ khóa : " + sTuKhoa;
            // Lấy tổng kết quả tìm kiếm
            ViewBag.sum = listKQ.Count();
            return View(listKQ.ToPagedList(pageNumber, pageSize));
        }

        // Thêm mới giảng viên
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            // Lấy username từ form submit
            string username = form["UserName"].ToString();
            // Kiểm tra username đã tồn tại trong DB chưa
            if (db.Users.Any(x => x.UserName.Equals(username)))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Khởi tạo đối tượng giảng viên và gán giá trị cho các thuộc tính
                Teacher tea = new Teacher();
                tea.UserName = form["UserName"].ToString();
                tea.Name = form["Name"].ToString();
                tea.TeacherCode = "123456";
                tea.Email = form["Email"].ToString();
                tea.PassWord = form["PassWord"].ToString();
                // Thêm giảng viên vào DB
                db.Teachers.Add(tea);
                db.SaveChanges();
                // Lấy mã giảng viên vừa thêm vào DB
                int teacherid = db.Teachers.Max(x => x.TeacherID);
                // Khởi tạo đối tượng user và gán giá trị cho thuộc tính
                User user = new User()
                {
                    UserName = tea.UserName,
                    PassWord = tea.PassWord,
                    Position = "Teacher",
                    TeacherID = teacherid
                };
                // Thêm user vào DB
                db.Users.Add(user);
                db.SaveChanges();
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
        }

        // Chỉnh sửa giảng viên theo id
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            // Lấy id giảng viên từ form
            int idTeacher = int.Parse(form["idTeacher"].ToString());
            // Lấy username từ form
            string username = form["UserName"].ToString();
            // Kiểm tra giảng username có  trùng lặp không
            if (db.Users.Any(x => x.UserName.Equals(username) && x.TeacherID != idTeacher))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Lấy ra user theo mã giảng viên và cập nhật dữ liệu
                User user = db.Users.FirstOrDefault(x => x.TeacherID == idTeacher);
                user.UserName = username;
                user.PassWord = form["PassWord"].ToString();
                // Lấy ra giảng viên theo mã và cập nhật dữ liệu
                Teacher tea = db.Teachers.FirstOrDefault(x => x.TeacherID == idTeacher);
                tea.UserName = username;
                tea.Name = form["Name"].ToString();
                tea.TeacherCode = "123456";
                tea.Email = form["Email"].ToString();
                tea.PassWord = form["PassWord"].ToString();
                db.SaveChanges();
                return Json(new { status = 2 }, JsonRequestBehavior.AllowGet);
            }
        }

        // Xóa giảng viên theo id
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy giảng viên theo id
            Teacher teacher = db.Teachers.FirstOrDefault(s => s.TeacherID == id);
            bool a = db.StudentDetails.Any(x => x.TeacherID == id);
            if (a)
            {
                return Json(new { status = 0, teacher = teacher }, JsonRequestBehavior.AllowGet);
            }
            if (teacher == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            return Json(new { status = 1, teacher = teacher }, JsonRequestBehavior.AllowGet);
        }

        // Xác nhận xóa theo id
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy giảng viên theo id
            Teacher teacher = db.Teachers.FirstOrDefault(s => s.TeacherID == id);
            if (teacher == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy user theo id của giảng viên
            User user = db.Users.FirstOrDefault(x => x.TeacherID == teacher.TeacherID);
            // Xóa user vừa lấy được
            db.Users.Remove(user);
            // Xóa giảng viên vừa lấy được
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        //Import danh sách giảng viên từ excel
        [HttpPost]
        public ActionResult ImportTeacher(HttpPostedFileBase fileUpload)
        {
            ViewBag.message = "Import không thành công";
            int count = 0;
            var package = new ExcelPackage(fileUpload.InputStream);
            if (ImportData(out count, package))
            {
                ViewBag.message = "Import thành công";
            }
            ViewBag.countStudent = count;
            return View();
        }

        // Import dữ liệu từ file
        public bool ImportData(out int count, ExcelPackage package)
        {
            count = 0;
            var result = false;
            try
            {
                // Bắt đầu từ cột 1
                int startColumn = 1;
                // Bắt đầu từ dòng 2
                int startRow = 2;
                // Lấy sheet 1 của file excel
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                object data = null;
                // Khởi tạo đối tượng DBcontext và thao tác csdl
                UetSurveyDbContext db = new UetSurveyDbContext();

                do
                {
                    // Lấy các dữ liệu của một dòng excel
                    data = workSheet.Cells[startRow, startColumn].Value;
                    object userName = workSheet.Cells[startRow, startColumn + 1].Value;
                    object passWord = workSheet.Cells[startRow, startColumn + 2].Value;
                    object fullName = workSheet.Cells[startRow, startColumn + 3].Value;
                    object email = workSheet.Cells[startRow, startColumn + 4].Value;
                    object checkColumn = workSheet.Cells[startRow, startColumn + 5].Value;
                    if (checkColumn != null)
                    {
                        throw new Exception("error");
                    }
                    // Kiểm tra xem đã cuối danh sách chưa
                    if (data != null)
                    {
                        var isSuccess = SaveTeacher(userName.ToString(), passWord.ToString(), fullName.ToString(), email.ToString(), db);
                        if (isSuccess)
                        {
                            count++;
                            result = true;
                        }
                    }
                    startRow++;
                } while (data != null);
            }
            catch (Exception x)
            {


            }
            return result;
        }

        // Lưu từng giảng viên vào DB
        public bool SaveTeacher(string userName, string passWord, string fullName, string email, UetSurveyDbContext db)
        {
            var result = false;
            try
            {
                // Kiểm tra giảng viên đã tồn tại chưa
                if (!db.Teachers.Any(x => x.UserName.Equals(userName)))
                {
                    // Khởi tạo giảng viên và gán giá trị cho các thuộc tính
                    Teacher teacher = new Teacher();
                    teacher.UserName = userName;
                    teacher.PassWord = passWord;
                    teacher.Name = fullName;
                    teacher.Email = email;
                    // Lưu giảng viên vào DB
                    db.Teachers.Add(teacher);
                    db.SaveChanges();
                    // Lấy mã giảng viên vừa tạo
                    int teacherid = db.Teachers.Max(x => x.TeacherID);
                    // Khởi tạo user và gán giá trị cho các thuộc tính
                    User user = new User()
                    {
                        UserName = userName,
                        PassWord = passWord,
                        Position = "Teacher",
                        TeacherID = teacherid
                    };
                    // Thêm user vào DB
                    db.Users.Add(user);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception x)
            {

            }
            return result;

        }
    }
}