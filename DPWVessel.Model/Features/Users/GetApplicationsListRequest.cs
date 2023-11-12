using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Collections.Generic;
using System.Linq;

namespace DPWVessel.Model.Features.Users
{
    public class GetApplicationsListRequest : IRequest<GetApplicationsListResponse>
    {

    }

    public class GetApplicationsListResponse : Response
    {
        public List<ApplicationsList> ApplicationsList { get; set; }
    }
    public class ApplicationsList
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public string AppCode { get; set; }
    }
    public class GetApplicationsListRequestHandler : IRequestHandler<GetApplicationsListRequest, GetApplicationsListResponse>
    {
        private dpw_VesselEntities _dbcontext;
        public GetApplicationsListRequestHandler(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }
        public GetApplicationsListResponse Execute(GetApplicationsListRequest request)
        {
            var row = _dbcontext.Applications.ToList();

            GetApplicationsListResponse resp = new GetApplicationsListResponse();
            resp.ApplicationsList = row.Select(x => new ApplicationsList
            {
                Id = x.Id,
                AppCode = x.AppCode,
                AppName = x.Name,
            }).ToList();

            return resp;
        }

    }
}
