using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Linq;

namespace DPWVessel.Model.Features.Students
{
    public class GetEquipmentTypeDetailRequest : IRequest<GetStudentsDetailResponse>
    {
        public int Id { get; set; }

    }
    public class GetStudentsDetailResponse : Response
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int rollNo { get; set; }
        public int age { get; set; }
    }

    public class GetStudentsRequestHandel : IRequestHandler<GetEquipmentTypeDetailRequest, GetStudentsDetailResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public GetStudentsRequestHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public GetStudentsDetailResponse Execute(GetEquipmentTypeDetailRequest request)
        {
            var req = _dbcontext.Students.FirstOrDefault(x => x.Sid == request.Id);
            GetStudentsDetailResponse resp = new GetStudentsDetailResponse();
            if (req != null)
            {

                resp.Id = req.Sid;
                resp.name = req.SName;
                resp.rollNo = req.RollNo;
                resp.age = req.Age;
            }
            return resp;

        }
    }

}
