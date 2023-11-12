using DE.Infrastructure.Concept;

namespace DPWVessel.Model.Features.Account
{
    public class ResetPasswordRequest : IRequest<ResetPasswordResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class ResetPasswordResponse : Response
    {

    }
}
