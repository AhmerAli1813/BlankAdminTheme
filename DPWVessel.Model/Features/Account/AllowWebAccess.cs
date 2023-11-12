using DE.Infrastructure.Concept;

namespace DPWVessel.Model.Features.Account
{
    public class AllowWebAccess : IRequest<AllowWebAccessResponse>
    {
        public string Id { get; set; }
        public bool Role { get; set; }
    }

    public class AllowWebAccessResponse : Response
    {
    }
}
