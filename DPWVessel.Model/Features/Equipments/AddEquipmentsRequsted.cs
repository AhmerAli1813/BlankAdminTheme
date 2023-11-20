using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Linq;

namespace DPWVessel.Model.Features.Equipments
{
    public class AddEquipmentsRequsted : IRequest<AddEquipmentsResponse>
    {
        public int id { get; set; }
        public string name { get; set; }
        public int equipmentsTypeId { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
    }
    public class AddEquipmentsResponse : Response
    {
        public bool IsTure { get; set; }
        public string message { get; set; }
    }

    public class AddEquipmentsRequstedHandel : IRequestHandler<AddEquipmentsRequsted, AddEquipmentsResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public AddEquipmentsRequstedHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public AddEquipmentsResponse Execute(AddEquipmentsRequsted request)
        {
            AddEquipmentsResponse rep = new AddEquipmentsResponse();
            var NameValidation = _dbcontext.Equipments.Where(x => x.Name == request.name && x.EquipmentTypeId == request.equipmentsTypeId).Count();
            if (NameValidation <= 0)
            {

                Equipment model = new Equipment
                {
                    Name = request.name,
                    EquipmentTypeId = request.equipmentsTypeId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.createdBy,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = request.updatedBy

                };

                _dbcontext.Equipments.Add(model);
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
