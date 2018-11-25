using System.Web.Mvc;

namespace UET_BTL_VERSION_1.Areas.Member
{
    public class MemberAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Member";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "TrangChuSinhVien",
               "sinh-vien/trang-chu/{id}",
               new {controller = "Student", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
               "ThongTinCaNhanSinhVien",
               "sinh-vien/thong-tin-ca-nhan/{id}",
               new {controller = "Student", action = "ShowInforStudent", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "DanhSachKhoaHocSinhVien",
                "sinh-vien/danh-sach-hoc-phan/{id}",
                new { controller = "Student", action = "ShowListSubject", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "DanhGiaHocPhan",
                "sinh-vien/danh-gia-hoc-phan/{id}",
                new { controller = "Student", action = "SurveySubject", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "TrangChuGiangVien",
                "giang-vien/trang-chu/{id}",
                new { controller = "Teacher", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "ThongTinCaNhanGiangVien",
                "giang-vien/thong-tin-ca-nhan/{id}",
                new { controller = "Teacher", action = "ShowInforTeacher", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "DanhSachHocPhanCuaGiangVien",
                "giang-vien/danh-sach-hoc-phan/{id}",
                new { controller = "Teacher", action = "ShowListSubject", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "DanhSachSinhVienHocPhan",
                "giang-vien/danh-sach-sinh-vien-theo-hoc-phan/{id}",
                new { controller = "Teacher", action = "ShowClass", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "KetQuaDanhGiaCaLop",
                "giang-vien/ket-qua-danh-gia-theo-hoc-phan/{id}",
                new { controller = "Teacher", action = "ShowResultSurvey", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Member_default",
                "Member/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}