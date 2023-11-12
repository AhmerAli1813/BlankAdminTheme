using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using DPWVessel.Model.Resources;
using FluentValidation;
using System.Linq;

namespace DPWVessel.Model.Features.Account
{
    public class EditProfileRequest : IRequest<EditProfileResponse>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FullNameAr { get; set; }
        public string Phone { get; set; }
    }

    public class EditProfileResponse : Response
    {

    }
    public class EditProfileRequestValidator : AbstractValidator<EditProfileRequest>
    {
        private readonly dpw_VesselEntities dpw_VesselEntities;
        public EditProfileRequestValidator(dpw_VesselEntities db)
        {
            dpw_VesselEntities = db;
            RuleFor(x => x.Email).NotEmpty().WithMessage(Messages.EmailRequired);
            RuleFor(x => x.FullName).NotEmpty().WithMessage(Messages.FullNameRequired);
            RuleFor(x => x.FullNameAr).NotEmpty().WithMessage(Messages.HazardNameArisRequired);
            RuleFor(x => x.Phone).NotEmpty().WithMessage(Messages.PhoneRequired);
            RuleFor(x => x).Must(EmailShouldNotRepeat).WithMessage("This Email Already Exits");
        }

        public bool EmailShouldNotRepeat(EditProfileRequest arg)
        {
            var count = dpw_VesselEntities.AspNetUsers.Where(y => y.Email == arg.Email && y.Id == arg.UserId).Count();
            if (count == 1)
            {
                return true;
            }
            else
            {
                int count1 = dpw_VesselEntities.AspNetUsers.Where(x => x.Email == arg.Email).Count();
                return count1 == 0;
            }
        }

    }


    public class EditProfileRequestHandler : IRequestHandler<EditProfileRequest, EditProfileResponse>
    {
        private dpw_VesselEntities _dbcontext;
        public EditProfileRequestHandler(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public EditProfileResponse Execute(EditProfileRequest request)
        {

            var row = _dbcontext.AspNetUsers.FirstOrDefault(x => x.Id == request.UserId);

            row.Email = request.Email;
            var row1 = _dbcontext.SiteUsers.FirstOrDefault(x => x.AspNetUserId == request.UserId);
            row1.FullName = request.FullName;

            return new EditProfileResponse();
        }


    }
}
