using System.Web;
using System.Web.Mvc;

namespace UET_BTL_VERSION_1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
