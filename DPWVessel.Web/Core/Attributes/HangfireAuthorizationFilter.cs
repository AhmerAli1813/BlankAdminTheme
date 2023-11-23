using Hangfire.Dashboard;
using System.Collections.Generic;
using System.Web;

namespace DPWVessel.Web.Core.Attributes
{

    public class HangfireAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return HttpContext.Current.User != null
                && HttpContext.Current.User.Identity.IsAuthenticated
                && HttpContext.Current.User.IsInRole("Admin");
        }
    }
}