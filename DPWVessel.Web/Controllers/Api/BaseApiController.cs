using DE.Infrastructure.Concept;
using System.Web.Http;

namespace DPWVessel.Web.Controllers.Api
{
    public class BaseApiController : ApiController
    {
        protected IRequestExecutor _requestExecutor;


    }
}
