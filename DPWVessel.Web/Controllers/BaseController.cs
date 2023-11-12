using System.Web.Mvc;

namespace DPWVessel.Web.Controllers
{
    [Authorize(Roles = "Admin,Operations,OperationManager,OperationApprover,StudyCommittee_GMPH_WGMPH,StudyCommittee_BERTH,DecisionCommittee,SteeringCommittee,Finance")]
    public class BaseController : Controller
    {

    }
}