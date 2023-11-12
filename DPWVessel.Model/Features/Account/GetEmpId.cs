using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Collections.Generic;

namespace DPWVessel.Model.Features.Account
{
    public class EmpIdRequest : IRequest<EmpIdResponse>
    {

        public int? OrganizationId { get; set; }

    }


    public class EmpIdResponse : Response
    {
        public List<EmpIdResponseDataModel> resp { get; set; }
    }
    public class EmpIdResponseDataModel
    {
        public int Id { get; set; }
        public string EpmStaffNo { get; set; }
        public string Name { get; set; }

    }

    public class EmpIdRequesttHandler : IRequestHandler<EmpIdRequest, EmpIdResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public EmpIdRequesttHandler(dpw_VesselEntities context)
        {
            _dbcontext = context;
        }

        public EmpIdResponse Execute(EmpIdRequest request)
        {

            return new EmpIdResponse();

        }

    }

}

