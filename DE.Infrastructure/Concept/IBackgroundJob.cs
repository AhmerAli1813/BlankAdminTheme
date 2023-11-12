using System.Threading.Tasks;

namespace DE.Infrastructure.Concept
{
    public interface IBackgroundJob
    {
        Task Execute();
    }
    public interface IBackgroundJob<T> : IBackgroundJob
    {
        Task Execute(T args);
    }
}
