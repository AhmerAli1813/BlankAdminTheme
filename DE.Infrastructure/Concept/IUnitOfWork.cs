using System;

namespace DE.Infrastructure.Concept
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
