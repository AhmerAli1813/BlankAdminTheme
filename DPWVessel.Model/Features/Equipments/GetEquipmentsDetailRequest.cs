using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Linq;

namespace DPWVessel.Model.Features.Equipments
{
    public class GetEquipmentsDetailRequest : IRequest<GetEquipmentsDetailResponse>
    {
        public int id { get; set; }

    }
    public class GetEquipmentsDetailResponse : Response
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? equipmentTypeId { get; set; }
        

    }

    public class GetEquipmentsRequestHandel : IRequestHandler<GetEquipmentsDetailRequest, GetEquipmentsDetailResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public GetEquipmentsRequestHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public GetEquipmentsDetailResponse Execute(GetEquipmentsDetailRequest request)
        {
            var req = _dbcontext.Equipments.FirstOrDefault(x => x.Id == request.id);
            GetEquipmentsDetailResponse resp = new GetEquipmentsDetailResponse();
            if (req != null)
            {

                resp.id = req.Id;
                resp.name = req.Name;
                resp.equipmentTypeId = req.EquipmentTypeId;
             
                
            }
            return resp;

        }
    }

}
