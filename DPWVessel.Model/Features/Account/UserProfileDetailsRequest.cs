﻿using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Linq;
namespace DPWVessel.Model.Features.Account
{
    public class UserProfileDetailsRequest : IRequest<UserProfileDetailsResponse>
    {
        public string UserId { get; set; }

    }
    public class UserProfileDetailsResponse : Response
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FullNameAr { get; set; }
        public string Phone { get; set; }

    }



    public class UserProfileDetailsRequestHandler : IRequestHandler<UserProfileDetailsRequest, UserProfileDetailsResponse>
    {
        private dpw_VesselEntities _dbcontext;
        public UserProfileDetailsRequestHandler(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public UserProfileDetailsResponse Execute(UserProfileDetailsRequest request)
        {
            var response = new UserProfileDetailsResponse();
            var row = _dbcontext.AspNetUsers.FirstOrDefault(x => x.Id == request.UserId);

            response.Email = row.Email;
            var row1 = _dbcontext.SiteUsers.FirstOrDefault(x => x.AspNetUserId == request.UserId);
            response.FullName = row1.FullName;

            return response;
        }
    }
}
