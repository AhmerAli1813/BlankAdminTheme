using System.Web;


namespace DPWVessel.Web.Helpers
{
    public static class EmailHelper
    {
        public static string RenderRazorViewToString(this System.Web.Mvc.Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new System.Web.Mvc.ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var sw = new System.IO.StringWriter())
            {
                var context = HttpContext.Current;
                var contextBase = new HttpContextWrapper(context);
                var routeData = new System.Web.Routing.RouteData();
                routeData.Values.Add("controller", controllerName);
                var controllerContext = new System.Web.Mvc.ControllerContext(contextBase, routeData, new Controllers.EmptyController());
                var razorViewEngine = new System.Web.Mvc.RazorViewEngine();


                var viewResult = razorViewEngine.FindPartialView(controllerContext, viewName, false);
                var viewContext = new System.Web.Mvc.ViewContext(controllerContext, viewResult.View,
                                                  new System.Web.Mvc.ViewDataDictionary(viewData),
                                                  new System.Web.Mvc.TempDataDictionary(),
                                                  sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}