using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Linq;

namespace DPWVessel.Model.Features.EquipmentTypes
{
    public class EquipmentTypeInformationUpdateRequest : IRequest<EquipmentTypesInformationUpdateResponse>
    {
        public EquipmentTypeData data { get; set; }
    }
    public class EquipmentTypesInformationUpdateResponse : Response
    {
        public bool IsTure { get; set; }
        public string message { get; set; }
    }
    public class EquipmentTypeData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string updatedBy { get; set; }
    }
    public class EquipmentTypesInformationUpdateRequestHandel : IRequestHandler<EquipmentTypeInformationUpdateRequest, EquipmentTypesInformationUpdateResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public EquipmentTypesInformationUpdateRequestHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public EquipmentTypesInformationUpdateResponse Execute(EquipmentTypeInformationUpdateRequest request)
        {
            EquipmentTypesInformationUpdateResponse rep = new EquipmentTypesInformationUpdateResponse();
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

