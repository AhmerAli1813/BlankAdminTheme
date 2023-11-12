using DE.Infrastructure.Concept;
using System.Net.Http;

namespace DPWVessel.Web.Core.Attributes
{
    public class ApiTransactionFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null)
            {
                if (actionExecutedContext.Request.Method == HttpMethod.Get)
                    return;
                var _unitOfWork = actionExecutedContext.Request.GetDependencyScope().GetService(typeof(IUnitOfWork)) as IUnitOfWork;
                _unitOfWork.Commit();
            }
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

        }
    }
}