using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Linq;

namespace DPWVessel.Model.Features.EquipmentTypes
{
    public class UpdateEquipmentsInformationRequest : IRequest<UpdateEquipmentsInformationResponse>
    {
        public EquipmentData data { get; set; }
    }
    public class UpdateEquipmentsInformationResponse : Response
    {
        public bool IsTure { get; set; }
        public string message { get; set; }
    }
    public class EquipmentData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string updatedBy { get; set; }
    }
    public class UpdateEquipmentInformationRequestHandel : IRequestHandler<UpdateEquipmentsInformationRequest,UpdateEquipmentsInformationResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public UpdateEquipmentInformationRequestHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public UpdateEquipmentsInformationResponse Execute(UpdateEquipmentsInformationRequest request)
        {
            UpdateEquipmentsInformationResponse rep = new UpdateEquipmentsInformationResponse();
            var NameValidation = _dbcontext.EquipmentTypes.Where(x => x.Name == request.data.name).Count();
            if (NameValidation <= 0)
            {

                var req = _dbcontext.EquipmentTypes.FirstOrDefault(x => x.Id == request.data.id);

                req.Name = request.data.name;
                req.UpdatedAt = DateTime.Now;
                req.UpdatedBy = request.data.updatedBy;
                _dbcontext.SaveChanges();
                rep.IsTure = true;
                rep.message = "Save";
                return rep;
            }
            else
            {
                rep.IsTure = false;
                rep.message = $"EquipmentTypes {request.data.name} already taken";
                return rep;
            }

        }
    }
}

