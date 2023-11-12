using Autofac;
using DE.Infrastructure.CodeContracts;
using DE.Infrastructure.Concept;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading.Tasks;

namespace DE.Infrastructure.ConceptImplementation
{
    public class RequestExecutor : IRequestExecutor
    {
        private readonly IComponentContext _context;
        public RequestExecutor(IComponentContext context)
        {
            _context = context;
        }

        public TResponse Execute<TResponse>(IRequest<TResponse> request)
            where TResponse : Response, new()
        {
            request.RequireNotNull("request");

            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            //first validate
            var validatorType = typeof(IValidator<>)
                .MakeGenericType(requestType);
            dynamic validator = ResolveOptional(validatorType);
            if (validator != null)
            {
                ValidationResult validationResult = (ValidationResult)validator.Validate((dynamic)request);
                if (!validationResult.IsValid)
                {
                    var response = new TResponse();
                    response.AddValidationErrors(validationResult.Errors);
                    return response;
                }
            }

            //run handler
            var handlerType = typeof(IRequestHandler<,>)
                .MakeGenericType(requestType, responseType);
            dynamic handler = Resolve(handlerType);

            return (TResponse)handler.Execute((dynamic)request);
        }

        public async Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request)
            where TResponse : Response, new()
        {
            request.RequireNotNull("request");

            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            //first validate
            var validatorType = typeof(IValidator<>)
                .MakeGenericType(requestType);
            dynamic validator = ResolveOptional(validatorType);

            if (validator != null)
            {
                ValidationResult validationResult = (ValidationResult)validator.Validate((dynamic)request);
                if (!validationResult.IsValid)
                {
                    var resp = new TResponse();
                    resp.AddValidationErrors(validationResult.Errors);
                    return resp;
                }
            }

            //run handler
            var handlerType = typeof(IRequestHandlerAsync<,>)
                    .MakeGenericType(requestType, responseType);
            dynamic handler = Resolve(handlerType);

            var response = await (dynamic)(handler.Execute((dynamic)request));
            return (TResponse)response;
        }

        private object Resolve(Type type)
        {
            return _context.Resolve(type);
        }

        private object ResolveOptional(Type type)
        {
            return _context.ResolveOptional(type);
        }

        private T Resolve<T>()
        {
            return _context.Resolve<T>();
        }

    }
}