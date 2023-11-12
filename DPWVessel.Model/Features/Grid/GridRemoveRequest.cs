using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using FluentValidation;

namespace DPWVessel.Model.Features.Grid
{
    public class GridRemoveRequest : IRequest<Response>
    {
        public object Entity { get; set; }
    }

    public class GridRemoveRequestValidator : AbstractValidator<GridRemoveRequest>
    {

    }

    public class GridRemoveRequestHandler : IRequestHandler<GridRemoveRequest, Response>
    {
        private dpw_VesselEntities _dbContext;
        public GridRemoveRequestHandler(dpw_VesselEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public Response Execute(GridRemoveRequest request)
        {
            _dbContext.Entry(request.Entity).State = System.Data.Entity.EntityState.Deleted;
            return new Response();
        }
    }
}
