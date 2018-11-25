using OfficeOpenXml;
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
    public class SubjectManagerController : Controller
    {
        // Khởi tạo DBcontext để thao tác với csdl
        private UetSurveyDbContext db = new UetSurveyDbContext();

        // Hiển thị danh sách các học phần
        public ActionResult Index()
        {
            return View(db.Subjects.ToList());
        }

        // Xóa học phần theo id
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra học phần theo id 
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Truyền dữ liệu ra view để xác nhận xóa
            return Json(subject, JsonRequestBehavior.AllowGet);
        }

        // Xác nhận xóa học phần theo id
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra học phần theo id
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Xóa học phần
            db.Subjects.Remove(subject);
            // Lấy ra danh sách các học sinh trong học phần đó
            IEnumerable<StudentDetail> list = db.StudentDetails.Where(s => s.SubjectID == id).ToList();
            // xóa các survey của các học sinh đó
            foreach (var item in list)
            {
                try
                {
                    db.Surveys.RemoveRange(item.Survey);
                }
                catch (Exception)
                {

                }
            }
            // Xóa các sinh viên khỏi lớp học phần đó
            db.StudentDetails.RemoveRange(list);
            db.SaveChanges();
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        // Import danh sách sinh viên của một học phần
        [HttpPost]
        public ActionResult ImportSubject(HttpPostedFileBase fileUpload)
        {
            ViewBag.message = "Import không thành công";
            int count = 0;
            var package = new ExcelPackage(fileUpload.InputStream);
            if (ImportData(out count, package))
            {
                ViewBag.message = "Import thành công";
            }
            return View();
        }

        // Import dữ liệu 
        public bool ImportData(out int count, ExcelPackage package)
        {
            count = 0;
            var result = false;
            try
            {
                // Bắt đầu từ cột 1
                int startColumn = 1;
                // Bắt đầu từ hàng 12
                int startRow = 12;
                // Lấy sheet 1 của file excel
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                object data = null;
                // Khởi tạo DBcontext đẻ thao tác csdl
                UetSurveyDbContext db = new UetSurveyDbContext();
                // Lấy các thông tin của học phần
                object teacherName = workSheet.Cells[7, 3].Value;
                object subjectName = workSheet.Cells[10, 3].Value;
                object subjectCode = workSheet.Cells[9, 3].Value;
                object classRoom = workSheet.Cells[8, 6].Value;
                object creditNumber = workSheet.Cells[9, 6].Value;
                object time = workSheet.Cells[8, 3].Value;
                // Kiểm tra xem học phần đó đã tồn tại chưa
                if (!db.Subjects.Any(x => x.SubjectCode.ToLower().Equals(subjectCode.ToString().ToLower())))
                {
                    // Khởi tạo học phần và gán các giá trị cho thuộc tính
                    Subject sub = new Subject();
                    sub.Name = subjectName.ToString();
                    sub.SubjectCode = subjectCode.ToString();
                    sub.ClassRoom = classRoom.ToString();
                    sub.CreditNumber = int.Parse(creditNumber.ToString());
                    sub.TimeTeach = time.ToString();
                    // lưu học phần vào DB
                    db.Subjects.Add(sub);
                    db.SaveChanges();

                    // Lấy id của học phần vừa thêm
                    int subID = db.Subjects.Max(x => x.SubjectID);
                    // Lấy ID của giáo viên
                    int teacherID = db.Teachers.FirstOrDefault(x => x.Name.ToLower().Equals(teacherName.ToString().ToLower())).TeacherID;

                    do
                    {
                        data = workSheet.Cells[startRow, startColumn].Value;
                        string userName = workSheet.Cells[startRow, startColumn + 1].Value.ToString();
                        object dob = workSheet.Cells[startRow, startColumn + 3].Value;
                        // Kiểm tra xem đã cuối danh sách chưa
                        if (data != null)
                        {
                            // Lấy ra sinh viên có username bằng username trong excel
                            Student stu = db.Students.FirstOrDefault(x => x.UserName.Trim().Equals(userName.Trim()));
                            // Kiểm tra thông tin ngày sinh và cập nhật cho sinh viên
                            if (stu.DateOfBirth == null)
                            {
                                stu.DateOfBirth = DateTime.Parse(dob.ToString());
                            }
                            // Kiểm tra mã sinh viên và cập nhật cho sinh viên
                            if (stu.StudentCode == null)
                            {
                                stu.StudentCode = userName;
                            }
                            // Khởi tạo đối tượng sinh viên chi tiết theo môn học
                            StudentDetail stuDetail = new StudentDetail();
                            stuDetail.StudentID = stu.StudentID;
                            stuDetail.SubjectID = subID;
                            stuDetail.TeacherID = teacherID;
                            // Lưu một sinh viên vào danh sách học phần
                            db.StudentDetails.Add(stuDetail);
                            db.SaveChanges();
                            result = true;
                        }
                        startRow++;
                    } while (data != null);
                }

            }
            catch (Exception x)
            {


            }
            return result;
        }

        // Hiển thị kết quả đánh giá theo cả lớp học phần
        public ActionResult ShowResultSurvey(int? id)
        {
            // Lấy ra danh sách các id sinh viên chi tiết của một học phần
            List<int> lis = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.Select(x => x.StudentDetailID).ToList();
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy số lượng sinh viên đã đánh giá học phần
            ViewBag.hasSurvey = db.Surveys.Where(x => lis.Any(k => k == x.StudentDetailID)).ToList().Count();
            // Lấy ra thông tin chi tiết của sinh viên đầu tiên trong học phần đó
            StudentDetail student_Detail = db.StudentDetails.First(x => x.SubjectID == id);
            // Lấy tổng số sinh viên của học phần đó
            ViewBag.SumStudent = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.ToList().Count();
            if (student_Detail == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            if (ViewBag.hasSurvey == 0)
            {
                return View(student_Detail);
            }
            // Lấy tổng số sinh viên của học phần đó
            ViewBag.SumStudent = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.ToList().Count();
            // Lấy ra điểm trung bình theo cả lớp của các tiêu chí
            ViewBag.ListPointAver = ViewBag.ListPointAver = db.Surveys
                .Where(x => lis.Any(k => k == x.StudentDetailID))
                .GroupBy(x => x.ContentSurveyID)
                .Select(x => x.Average(y => y.Point)).ToList();
            // Lấy ra danh sách các tiêu chí đánh giá
            ViewBag.NameSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            // Lấy ra tổng số tiêu chí đánh giá
            ViewBag.CountSurvey = db.ContentSurveys.ToList().Count();
            return View(student_Detail);
        }

        // Hiển thị danh sách sinh viên theo học phần
        public ActionResult ShowClass(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra danh sách sinh viên của học phần rồi truyền qua view
            IEnumerable<StudentDetail> listStudent = db.StudentDetails.Where(s => s.SubjectID == id).ToList();
            if (listStudent == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            return View(listStudent);
        }

        // Hiển thị kết quả đánh giá của từng sinh viên với học phần tương ứng
        public ActionResult ResultSurveyEveryStudent(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Kiểm tra xem sinh viên đã đánh giá chưa
            ViewBag.hasSurvey = db.Surveys.Where(x => x.StudentDetailID == id).ToList().Count();
            if (ViewBag.hasSurvey == 0)
            {
                return View();
            }
            // Lấy ra các kết quả đánh giá của sinh viên về học phần đó
            ViewBag.ListPoint = db.Surveys
              .Where(x => x.StudentDetailID == id)
              .Select(x => x.Point).ToList();
            if (ViewBag.ListPoint == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra các tiêu chí đánh giá để hiển thị trong view
            ViewBag.NameSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            // Lấy tổng số tiêu chí
            ViewBag.CountSurvey = db.ContentSurveys.ToList().Count();
            // Lấy ra  phản hồi của sinh viên về học phần đó
            ViewBag.note = db.StudentDetails.SingleOrDefault(x => x.StudentDetailID == id).NoteSurvey;
            return View();

        }

    }
}