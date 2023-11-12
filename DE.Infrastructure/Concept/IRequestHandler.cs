namespace DE.Infrastructure.Concept
{
    public interface IRequestHandler<in TRequest, out TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Response, new()
    {
        TResponse Execute(TRequest request);
    }
}
