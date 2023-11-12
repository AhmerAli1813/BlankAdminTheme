using Autofac;
using FluentScheduler;

namespace DPWVessel.Web.Core.Jobs
{
    public class FluentJobFactory : IJobFactory
    {
        private readonly IContainer _context;

        public FluentJobFactory(IContainer context)
        {
            _context = context;
        }

        public IJob GetJobInstance<T>() where T : IJob
        {
            return _context.Resolve<T>();
        }
    }
}