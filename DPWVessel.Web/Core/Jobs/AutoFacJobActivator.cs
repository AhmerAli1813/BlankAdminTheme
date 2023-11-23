using Autofac;
using Autofac.Core.Lifetime;
using Hangfire;
using Hangfire.Annotations;
using System;

namespace DPWVessel.Web.Core.Jobs
{
    public class AutofacJobActivator : JobActivator
    {
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacJobActivator"/>
        /// class with the given Autofac Lifetime Scope.
        /// </summary>
        /// <param name="lifetimeScope">Container that will be used to create instance
        /// of classes during job activation process.</param>
        public AutofacJobActivator([NotNull] ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null) throw new ArgumentNullException("lifetimeScope");
            _lifetimeScope = lifetimeScope;
        }

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            return _lifetimeScope.Resolve(jobType);
        }

        [Obsolete]
        public override JobActivatorScope BeginScope()
        {
            return new AutofacScope(_lifetimeScope.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag));
        }

        class AutofacScope : JobActivatorScope
        {
            private readonly ILifetimeScope _lifetimeScope;

            public AutofacScope(ILifetimeScope lifetimeScope)
            {
                _lifetimeScope = lifetimeScope;
            }

            public override object Resolve(Type type)
            {
                return _lifetimeScope.Resolve(type);
            }

            public override void DisposeScope()
            {
                _lifetimeScope.Dispose();
            }
        }
    }
}