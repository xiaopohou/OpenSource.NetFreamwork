using System.Web;
using System.Web.Mvc;

namespace OpenSource.Web.Listed
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
