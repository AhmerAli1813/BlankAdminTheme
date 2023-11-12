using System.Web.Mvc;

namespace DPWVessel.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new Core.Attributes.HandleErrorAttribute());
        }
    }
}
