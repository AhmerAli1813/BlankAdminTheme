using System.Threading.Tasks;

namespace DE.Infrastructure.Concept
{
    public interface IRequestExecutor
    {
        TResponse Execute<TResponse>(IRequest<TResponse> request)
            where TResponse : Response, new();

        Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request)
            where TResponse : Response, new();

    }

}
