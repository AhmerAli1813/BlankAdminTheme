using DPWVessel.Web.Core.Session;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DPWVessel.Web.Core.Attributes
{
    public class GlobalPropertiesActionFilter : IActionFilter
    {
        public class JsUserObject
        {
            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public IEnumerable<string> UserType { get; set; }
            public int Id { get; set; }
            public List<string> UsersApplication { get; set; }
        }
        public ISessionManager _sessionMangaer { get; set; }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_sessionMangaer.CurrentUser != null)
            {
                //strip properties so that it can be sent to client side via JS
                var userObject = new JsUserObject
                {
                    FullName = _sessionMangaer.CurrentUser.FullName,
                    UserName = _sessionMangaer.CurrentUser.UserName,
                    Email = _sessionMangaer.CurrentUser.Email,
                    UserType = _sessionMangaer.CurrentUser.UserType,
                    UsersApplication = _sessionMangaer.CurrentUser.UsersApplication,
                    Id = _sessionMangaer.CurrentUser.UserId
                };

                filterContext.Controller.ViewBag.CurrentUser = userObject;
            }
        }
    }
}