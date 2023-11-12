using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Linq;

namespace DPWVessel.Model.Features.Account
{
    public class GetSiteuserRequest : IRequest<GetSiteuserResponse>
    {
        public string AspNetUserId { get; set; }
    }
    public class GetSiteuserResponse : Response
    {
        public int Id { get; set; }
    }
    public class GetSiteuserRequestHandler : IRequestHandler<GetSiteuserRequest, GetSiteuserResponse>
    {
        private dpw_VesselEntities _dbcontext;
        public GetSiteuserRequestHandler(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }
        public GetSiteuserResponse Execute(GetSiteuserRequest request)
        {
            var row = _dbcontext.SiteUsers.FirstOrDefault(x => x.AspNetUserId == request.AspNetUserId);
            GetSiteuserResponse resp = new GetSiteuserResponse();
            resp.Id = row.Id;

            return resp;
        }

    }

}
