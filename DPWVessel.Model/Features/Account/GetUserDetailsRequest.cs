using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Linq;

namespace DPWVessel.Model.Features.Account
{
    public class GetUserDetailsRequest : IRequest<GetUserDetailsResponse>
    {
        public int Id { get; set; }

    }
    public class GetUserDetailsResponse : Response
    {
        public string AspnetUserId { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public string Phone { get; set; }
        public string UserImage { get; set; }
        public string[] UserType { get; set; }
        public string[] UsersApplication { get; set; }
        //public List<UsersApplication> UsersApplication { get; set; }

    }
    public class UsersApplication
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public bool IsChecked { get; set; }
    }

    public class GetUserDetailsRequestHandler : IRequestHandler<GetUserDetailsRequest, GetUserDetailsResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public GetUserDetailsRequestHandler(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public GetUserDetailsResponse Execute(GetUserDetailsRequest request)
        {
            var user = _dbcontext.SiteUsers.FirstOrDefault(x => x.Id == request.Id);
            var aspnetUser = user.AspNetUser;
            var resp = new GetUserDetailsResponse();
            resp.Id = user.Id;
            resp.Email = aspnetUser.Email;
            resp.Username = aspnetUser.UserName;
            resp.FullName = user.FullName;
            resp.UserImage = user.UserImage;
            //    resp.UserType = Enum.Parse(typeof(UserRole), user.UserType.ToString()).ToString();
            resp.UserType = user.AspNetUser.AspNetRoles.Select(v => v.Id).ToArray();
            resp.UsersApplication = user.UsersApplications.Select(v => v.Application.Id.ToString()).ToArray();
            resp.AspnetUserId = aspnetUser.Id;
            resp.Status = user.Status;
            resp.Phone = user.Phone;

            // new code
            //var App = from u in _dbcontext.Applications
            //              select u;
            //resp.UsersApplication= (from A in App
            //                        join UA in _dbcontext.UsersApplications.Where(x => x.UserId == request.Id)
            //             on A.Id equals UA.AppId into result
            //                   from x in result.DefaultIfEmpty()
            //                   select new UsersApplication
            //                   {
            //                       AppId = A.Id,
            //                       AppName = A.Name,
            //                       IsChecked = (x != null),
            //                   }).ToList();

            //response.FormData = obj;
            return resp;
        }
    }
}
