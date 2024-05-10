using System.Web;
using System.Web.Mvc;

namespace ThanhGiang_WebCuoiKi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
