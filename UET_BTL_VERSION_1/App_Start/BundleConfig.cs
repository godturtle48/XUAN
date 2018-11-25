using System.Web;
using System.Web.Optimization;

namespace UET_BTL_VERSION_1
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bootstrap/css")
                .Include(
                       "~/Assets/Shared/css/bootstrap.min.css",
                       new CssRewriteUrlTransform()
                        )
               .Include(
                       "~/Assets/Shared/css/dashboard.css"
                        ));
                
            bundles.Add(new ScriptBundle("~/bootstrap-jquery/js").Include(
                      "~/Assets/Shared/js/jquery-3.3.1.min.js",
                      "~/Assets/Shared/js/bootstrap.min.js"
                      ));
            bundles.Add(new ScriptBundle("~/teacher/js").Include(
                     "~/Assets/Admin/js/teacherManager.js"
                     ));
            bundles.Add(new ScriptBundle("~/student/js").Include(
                     "~/Assets/Admin/js/studentManager.js"
                     ));
            bundles.Add(new ScriptBundle("~/subject/js").Include(
                     "~/Assets/Admin/js/subjectManager.js"
                     ));
            bundles.Add(new ScriptBundle("~/survey/js").Include(
                     "~/Assets/Admin/js/surveyManager.js"
                     ));
            bundles.Add(new ScriptBundle("~/login/js").Include(
                     "~/Assets/SignIn/js/login.js"
                     ));

            bundles.Add(new StyleBundle("~/login/css").Include(
                     "~/Assets/SignIn/css/Login.css"
                     ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
