using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DPWVessel.Web.Core.Attributes;
using DPWVessel.Web.Core.IoC;
using DPWVessel.Web.Core.Jobs;
using Hangfire;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Threading.Tasks;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(DPWVessel.Web.Startup))]
namespace DPWVessel.Web
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var component = ConfigureIoc(app);
            ConfigureAuth(app);
            ConfigureFluentScheduler(app, component);
        }

        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();
            context.Response.Redirect("/?errormessage=" + context.Exception.Message);
            return Task.FromResult(0);
        }

        private static IContainer ConfigureIoc(IAppBuilder app)
        {
            var config = System.Web.Http.GlobalConfiguration.Configuration;

            var builder = new ContainerBuilder();
            builder = IoC.InitializeAutoFac(builder);
            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);

            return container;
        }

        private static void ConfigureFluentScheduler(IAppBuilder app, IContainer container)
        {
            var registry = new FluentScheduler.Registry();
            //ConfigurationManager.AppSettings[""]
            // registry.Schedule<JobExecutor<ReportScheduler>>().NonReentrant().ToRunEvery(1).Days();
            FluentScheduler.JobManager.InitializeWithoutStarting(registry);
            FluentScheduler.JobManager.JobFactory = new FluentJobFactory(container);
            FluentScheduler.JobManager.Start();
        }

    
        private static void ConfireHangfire(IAppBuilder app, IContainer container)
        {
            Hangfire.GlobalConfiguration.Configuration.UseActivator(new AutofacJobActivator(container));
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireServer();

            // Map Dashboard to the `http://<your-app>/hangfire` URL.
            app.UseHangfireDashboard("/hangfire", new DashboardOptions { AuthorizationFilters = new[] { new HangfireAuthorizationFilter() } });

        }
    }
}
