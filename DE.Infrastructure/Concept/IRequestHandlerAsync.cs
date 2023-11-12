using System.Threading.Tasks;

namespace DE.Infrastructure.Concept
{
    public interface IRequestHandlerAsync<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Response, new()
    {
        Task<TResponse> Execute(TRequest request);
    }
}
