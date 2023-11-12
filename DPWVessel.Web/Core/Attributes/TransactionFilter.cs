using DE.Infrastructure.Concept;
using System.Net.Http;
using System.Web.Mvc;

namespace DPWVessel.Web.Core.Attributes
{
    public class TransactionFilter : IResultFilter
    {
        public IUnitOfWork _unitOfWork { get; set; }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod == HttpMethod.Get.Method)
                return;
            if (filterContext.Exception == null || filterContext.ExceptionHandled)
                _unitOfWork.Commit();
        }
    }
}
