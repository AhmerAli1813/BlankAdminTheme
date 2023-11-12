using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using DPWVessel.Web.Core.Session;

namespace DPWVessel.Web.Controllers
{

    public class ReportsController : BaseController
    {

        private dpw_VesselEntities dbcontext;
        private readonly IRequestExecutor _requestExecutor;
        private ISessionManager _sessionManager;
        public ReportsController(IRequestExecutor requestExecutor, ISessionManager sessionManager, dpw_VesselEntities _context)
        {
            _requestExecutor = requestExecutor;
            _sessionManager = sessionManager;
            dbcontext = _context;

        }


    }
}