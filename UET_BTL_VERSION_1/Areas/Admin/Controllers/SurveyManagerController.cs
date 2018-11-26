using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Authority;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.Admin.Controllers
{
    [AuthorizeBusiness]
    public class SurveyManagerController : Controller
    {
        // Khởi tạo đối tượng DBcontext để thao các csdl
        private UetSurveyDbContext db = new UetSurveyDbContext();

        // Hiển thị các tiêu chí đánh giá
        public ActionResult Index()
        {
            return View(db.ContentSurveys.ToList());
        }

        // Thêm mới tiêu chí đánh giá được submit tư form
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            // Lấy ra tiêu chí đánh giá từ form
            string text = form["Text"].ToString();
            // Kiểm tra xem tiêu chí đó đã trùng chưa
            if (db.ContentSurveys.Any(x => x.Text.Equals(text)))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Khởi tạo một survey và lưu vào DB
                ContentSurvey content = new ContentSurvey();
                content.Text = text;
                db.ContentSurveys.Add(content);
                // Xóa bỏ các điểm đánh giá của sinh viên đối với các tiêu chí cũ
                db.Surveys.RemoveRange(db.Surveys.ToList());
                db.SaveChanges();
                // Lấy id của tiêu chí vừa tạo
                int lastId = db.ContentSurveys.Max(s => s.ContentSurveyID);
                return Json(new { status = 1, content = content, id = lastId }, JsonRequestBehavior.AllowGet);
            }
        }

        // Chỉnh sửa tiêu chí theo id
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra tiêu chí theo id
            ContentSurvey contentSurvey = db.ContentSurveys.FirstOrDefault(s => s.ContentSurveyID == id);
            if (contentSurvey == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Truyền tiêu chí đó sang view để sửa
            return View(contentSurvey);
        }

        // Xác nhận sửa từ form
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            // Lấy tiêu chí được submit từ form
            string text = form["Text"].ToString();
            // Lấy id của tiêu chí đó
            int idContentSurvey = int.Parse(form["idContentSurvey"].ToString());
            // kiểm tra xem tiêu chí đó có trùng lặp không
            if (db.ContentSurveys.Any(x => x.Text.Equals(text) && x.ContentSurveyID != idContentSurvey))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Lấy ra  tiêu chí đó trong DB rồi cập nhật
                ContentSurvey content = db.ContentSurveys.FirstOrDefault(x => x.ContentSurveyID == idContentSurvey);
                content.Text = text;
                db.SaveChanges();
                return Json(new { status = 2 }, JsonRequestBehavior.AllowGet);
            }
        }

        // Xóa tiêu chí đánh giá theo id
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra tiêu chí theo id
            ContentSurvey contentSurvey = db.ContentSurveys.FirstOrDefault(x => x.ContentSurveyID == id);
            if (contentSurvey == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            return Json(contentSurvey, JsonRequestBehavior.AllowGet);
        }

        // Xác nhận xóa tiêu chí đánh giá theo id
        [HttpPost]
        public ActionResult Delete(int id)
        {
           
            // Láy ra tiêu chí đó và xóa
            ContentSurvey contentSurvey = db.ContentSurveys.FirstOrDefault(s => s.ContentSurveyID == id);
            if (contentSurvey == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            db.ContentSurveys.Remove(contentSurvey);
            try
            {
                // Lấy ra các sinh viên có tiêu chí đó và xóa
                IEnumerable<Survey> listsurvey = db.Surveys.Where(x => x.ContentSurveyID == id);
                db.Surveys.RemoveRange(listsurvey);
            }
            catch (Exception)
            {
            }
            db.SaveChanges();
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}