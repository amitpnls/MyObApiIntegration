using System.Web;
using System.Web.Mvc;

namespace HonourIT_Xero_MyOB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
