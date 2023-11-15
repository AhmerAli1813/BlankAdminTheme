using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DPWVessel.Model.Features.EquipmentTypes
{
    public class GetEquipmentTypeDetailRequest : IRequest<GetEquipmentTypesDetailResponse>
    {
        public int id { get; set; }

    }
    public class GetEquipmentTypesDetailResponse : Response
    {
        public int id { get; set; }
        public string name { get; set; }
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }
    }

    public class GetEquipmentTypesRequestHandel : IRequestHandler<GetEquipmentTypeDetailRequest, GetEquipmentTypesDetailResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public GetEquipmentTypesRequestHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public GetEquipmentTypesDetailResponse Execute(GetEquipmentTypeDetailRequest request)
        {
            var req = _dbcontext.EquipmentTypes.FirstOrDefault(x => x.Id == request.id);
            GetEquipmentTypesDetailResponse resp = new GetEquipmentTypesDetailResponse();
            if (req != null)
            {

                resp.id = req.Id;
                resp.name = req.Name;
                resp.createdAt = req.CreatedAt;
                resp.createdBy = req.CreatedBy;
                resp.updatedAt = req.UpdatedAt;
                resp.updatedBy = req.UpdatedBy;

                
            }
            return resp;

        }
    }

}
