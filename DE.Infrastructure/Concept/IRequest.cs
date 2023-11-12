namespace DE.Infrastructure.Concept
{
    public interface IRequest
    {
    }

    public interface IRequest<TResponse> : IRequest
        where TResponse : Response, new()
    {
    }
}
