using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace DPWVessel.Model.Features.Equipments
{
    public class GetEquipmentsPrintRequsted : IRequest<GetEquipmentsPrintRespone>
    {
        public int? id { get; set; }


    }
    public class GetEquipmentsPrintRespone : Response
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? equipmentTypeId { get; set; }
        public string equipmentTypeName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd-MM-yyyy}")]
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }

    }


    public class EquipmentsPrintListRequstedHandel : IRequestHandler<GetEquipmentsPrintRequsted, GetEquipmentsPrintRespone>
    {
        private dpw_VesselEntities _dbcontext;
        public EquipmentsPrintListRequstedHandel(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public GetEquipmentsPrintRespone Execute(GetEquipmentsPrintRequsted request)
        {

            var row = _dbcontext.Equipments.Include(e => e.EquipmentType).FirstOrDefault(x => x.Id == request.id);
            GetEquipmentsPrintRespone rep = new GetEquipmentsPrintRespone();

            rep.id = row.Id;
            rep.name = row.Name;
            rep.equipmentTypeId = row.EquipmentTypeId;
            rep.equipmentTypeName = row.EquipmentType.Name;
            rep.createdAt = row.CreatedAt;
            rep.createdBy = row.CreatedBy;


            return rep;
        }
    }
}
