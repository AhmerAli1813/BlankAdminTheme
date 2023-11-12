using DE.Infrastructure.Concept;
using DE.Infrastructure.Helpers;
using DPWVessel.Model.EntityModel;
using FluentValidation;
using FluentValidation.Results;

namespace DPWVessel.Model.Features.Grid
{
    public class GridUpdateRequest<T> : IRequest<GridUpdateResponse<T>>
    {
        public T Entity { get; set; }
    }

    public class GridUpdateResponse<T> : Response
    {
        public T Entity { get; set; }
    }

    public class GridUpdateRequestValidator<T> : AbstractValidator<GridUpdateRequest<T>>
    {
        private IValidator<T> _entityValidator;
        public GridUpdateRequestValidator(IValidator<T> entityValidator)
        {
            _entityValidator = entityValidator;
        }
        public override ValidationResult Validate(GridUpdateRequest<T> instance)
        {
            return _entityValidator.Validate(instance.Entity);
        }
    }

    public class GridUpdateRequestHandler<T> : IRequestHandler<GridUpdateRequest<T>, GridUpdateResponse<T>> where T : class, DomainModel
    {
        private dpw_VesselEntities _dbContext;
        public GridUpdateRequestHandler(dpw_VesselEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public GridUpdateResponse<T> Execute(GridUpdateRequest<T> request)
        {
            _dbContext.Entry(request.Entity).State = System.Data.Entity.EntityState.Modified;
            return new GridUpdateResponse<T> { Entity = request.Entity };
        }
    }
}
