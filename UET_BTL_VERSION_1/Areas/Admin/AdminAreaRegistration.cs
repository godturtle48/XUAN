using System.Web.Mvc;

namespace UET_BTL_VERSION_1.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
                context.MapRoute(
                   "TrangChu",
                   "quan-ly/trang-chu/{id}",
                   new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "DanhSachSinhVien",
                    "quan-ly-sinh-vien/danh-sach-sinh-vien/{id}",
                    new { controller = "StudentManager", action = "Index", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "ImportSinhVien",
                    "quan-ly-sinh-vien/import-sinh-vien/{id}",
                    new { controller = "StudentManager", action = "ImportStudent", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "DanhSachGiangVien",
                    "quan-ly-giang-vien/danh-sach-giang-vien/{id}",
                    new { controller = "TeacherManager", action = "Index", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "ImportGiangVien",
                    "quan-ly-giang-vien/import-giang-vien/{id}",
                    new { controller = "TeacherManager", action = "ImportTeacher", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "DanhSachHocPhan",
                    "quan-ly-hoc-phan/danh-sach-hoc-phan/{id}",
                    new { controller = "SubjectManager", action = "Index", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "ImportHocPhan",
                    "quan-ly-hoc-phan/import-hoc-phan/{id}",
                    new { controller = "SubjectManager", action = "ImportSubject", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "KetQuaDanhGia",
                    "quan-ly-hoc-phan/ket-qua-danh-gia-tong-hop/{id}",
                    new { controller = "SubjectManager", action = "ShowResultSurvey", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "DanhSachSinhVienTheoHocPhan",
                    "quan-ly-hoc-phan/danh-sach-sinh-vien/{id}",
                    new { controller = "SubjectManager", action = "ShowClass", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "KetQuaDanhGiaTungSinhVien",
                    "quan-ly-hoc-phan/ket-qua-danh-gia-cua-tung-sinh-vien/{id}",
                    new { controller = "SubjectManager", action = "ResultSurveyEveryStudent", id = UrlParameter.Optional }
                );
                context.MapRoute(
                   "DanhSachTieuChi",
                   "quan-ly-danh-gia/danh-sach-tieu-chi/{id}",
                   new { controller = "SurveyManager", action = "Index", id = UrlParameter.Optional }
                );
                context.MapRoute(
                    "Admin_default",
                    "Admin/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
            }
    }
}