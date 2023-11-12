using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DPWVessel.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<dpw_VesselEntities>());
            System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            System.Web.Http.GlobalConfiguration.Configuration.Formatters.Remove(System.Web.Http.GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            string culture = "en-US";
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                culture = langCookie.Value;
            }
            else if (Request.UserLanguages != null)
            {
                culture = Request.UserLanguages[0];
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        }
    }
}
