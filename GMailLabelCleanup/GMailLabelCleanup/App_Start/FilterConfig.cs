using System.Web;
using System.Web.Mvc;
using GMailLabelCleanup.Core.Attributes.ActionFilters;

namespace GMailLabelCleanup
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new InsightsErrorAttribute(), 500);
            filters.Add(new HandleErrorAttribute(), 1000);
            filters.Add(new RequireHttpsAttribute(), 2000);
            filters.Add(new AppSettingsAttribute(), 3000);
        }
    }
}
