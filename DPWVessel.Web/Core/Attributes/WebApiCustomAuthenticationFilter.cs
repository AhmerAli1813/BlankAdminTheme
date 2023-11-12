using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace DPWVessel.Web.Core.Attributes
{
    public class WebApiCustomAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public const string AuthHeaderKey = "X-Auth-Token";
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authHeaderValue = GetAuthHeaderValue(context.Request);
            if (!string.IsNullOrEmpty(authHeaderValue))
            {
                var username = authHeaderValue;// Encryption.Decrypt(authHeaderValue);

                var userManager = context.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindByIdAsync(username);
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user.Id);
                    IPrincipal genericPrincipal = new GenericPrincipal(new GenericIdentity(user.UserName, "Forms"), roles.ToArray());
                    context.Principal = genericPrincipal;
                }
            }
        }
        private string GetAuthHeaderValue(HttpRequestMessage request)
        {
            var authHeader = request.Headers.SingleOrDefault(h => h.Key == AuthHeaderKey);
            if (authHeader.Value != null)
            {
                return authHeader.Value.FirstOrDefault();
            }
            authHeader = request.Headers.SingleOrDefault(h => h.Key == AuthHeaderKey.ToUpperInvariant());
            if (authHeader.Value != null)
            {
                return authHeader.Value.FirstOrDefault();
            }
            authHeader = request.Headers.SingleOrDefault(h => h.Key == AuthHeaderKey.ToLowerInvariant());
            if (authHeader.Value != null)
            {
                return authHeader.Value.FirstOrDefault();
            }
            return null;
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                IPrincipal incomingPrincipal = context.ActionContext.RequestContext.Principal;
            });
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}