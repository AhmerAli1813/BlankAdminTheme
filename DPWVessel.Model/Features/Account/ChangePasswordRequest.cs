using DE.Infrastructure.Concept;

namespace DPWVessel.Model.Features.Account
{
    class ChangePasswordRequest : IRequest<ChangePasswordResponse>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordResponse : Response
    {
    }


}
