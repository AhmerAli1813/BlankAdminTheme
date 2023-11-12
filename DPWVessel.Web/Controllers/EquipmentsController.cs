using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using DPWVessel.Web.Core.Session;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace DPWVessel.Web.Controllers
{
    /// <summary>
    [Authorize(Roles = "Admin")]
    /// </summary>
    public class EquipmentsController : BaseController
    {
        private readonly IRequestExecutor _requestExecutor;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly dpw_VesselEntities _dbContext;
        private readonly ISessionManager _sessionManager;

        public EquipmentsController(dpw_VesselEntities dbContext, ISessionManager sessionManager, IRequestExecutor requestExecutor)
        {
            _dbContext = dbContext;
            _sessionManager = sessionManager;
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _signInManager = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            _requestExecutor = requestExecutor;

        }
        // GET: EquipmentTypes
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Add()
        {

            return View();
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Step2()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ImportUserData()
        {

            return View();
        }

    }
}