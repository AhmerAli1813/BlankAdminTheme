using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Linq;

namespace DPWVessel.Model.Features.EquipmentTypes
{
    public class AddEquipmentsTypesRequsted : IRequest<AddEquipmentTypesResponse>
    {
        public int id { get; set; }
        public string name { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
    }
    public class AddEquipmentTypesResponse : Response
    {
        public bool IsTure { get; set; }
        public string message { get; set; }
    }

    public class AddEquipmentTypesRequstedHandel : IRequestHandler<AddEquipmentsTypesRequsted, AddEquipmentTypesResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public AddEquipmentTypesRequstedHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public AddEquipmentTypesResponse Execute(AddEquipmentsTypesRequsted request)
        {
            AddEquipmentTypesResponse rep = new AddEquipmentTypesResponse();
            var NameValidation = _dbcontext.EquipmentTypes.Where(x => x.Name == request.name).Count();
            if (NameValidation <= 0)
            {

                EquipmentType model = new EquipmentType
                {
                    Name = request.name,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.createdBy,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = request.updatedBy

                };

                _dbcontext.EquipmentTypes.Add(model);
                _dbcontext.SaveChanges();
                rep.IsTure = true;
                rep.message = "Save";
                return rep;
            }
            else
            {
                rep.IsTure = false;
                rep.message = $"Equipment Types : {request.name} already taken";
                return rep;
            }
        }
    }
}
