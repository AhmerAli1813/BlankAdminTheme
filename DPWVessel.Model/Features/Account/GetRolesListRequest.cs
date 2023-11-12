using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Collections.Generic;
using System.Linq;

namespace DPWVessel.Model.Features.Account
{
    public class GetRolesListRequest : IRequest<GetRolesListResponse>
    {

    }
    public class GetRolesListResponse : Response
    {

        public List<RolesList> RolesList { get; set; }
    }
    public class RolesList
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class GetRolesListRequestHandler : IRequestHandler<GetRolesListRequest, GetRolesListResponse>
    {
        private dpw_VesselEntities _dbcontext;
        public GetRolesListRequestHandler(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }
        public GetRolesListResponse Execute(GetRolesListRequest request)
        {
            var row = _dbcontext.AspNetRoles.AsQueryable();
            GetRolesListResponse resp = new GetRolesListResponse();
            resp.RolesList = _dbcontext.AspNetRoles.Select(x => new RolesList
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

            return resp;
        }

    }
}
