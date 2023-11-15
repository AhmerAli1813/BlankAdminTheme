using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace DPWVessel.Model.Features.Equipments
{
    public class GetAllEquipmentsRequsted : IRequest<EquipmentsRespone>
    {
        public int? id { get; set; }
        public string name { get; set; }
        public int? equipmentTypeId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
       
        public GetAllEquipmentsRequsted()
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;
        }


    }
    public class EquipmentsRespone : Response
    {
        
        public List<EquipmentsList> EquipmentsLists { get; set; }
    }
    public class EquipmentsList
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? equipmentTypeId { get; set; }
        public string equipmentTypeName { get; set; }
        
        public DateTime createdAt { get; set; } 
        public string createdBy { get; set; }
        
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }
        
    }
    public class EquipmentsListRequstedHandel : IRequestHandler<GetAllEquipmentsRequsted, EquipmentsRespone>
    {
        private dpw_VesselEntities _dbcontext;
        public EquipmentsListRequstedHandel(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public EquipmentsRespone Execute(GetAllEquipmentsRequsted request)
        {
            EquipmentsRespone rep = new EquipmentsRespone();
            var row = _dbcontext.Equipments.Include(e=>e.EquipmentType).ToList();
                
               
            if (request.id > 0)
            {
                row = row.Where(x => x.Id == request.id).ToList();
            }
            if (request.equipmentTypeId > 0)
            {
                row = row.Where(x => x.EquipmentTypeId == request.equipmentTypeId).ToList();
            }
            if (request.name != "" && request.name != null)
            {
                row = row.Where(x => x.Name == request.name).ToList();
            }
            
            if (request.startDate != DateTime.MinValue)
            {
                row = row.Where(x => x.CreatedAt >= request.startDate).ToList();
            }
            if (request.endDate != DateTime.MinValue)
            {
                request.endDate = request.endDate.AddDays(1);
                row = row.Where(x => x.CreatedAt <= request.endDate).ToList();
            }
                        
            

            rep.EquipmentsLists = row.Select(x => new EquipmentsList
            {
                id = x.Id,
                name = x.Name,
                equipmentTypeId = x.EquipmentTypeId,
                equipmentTypeName = x.EquipmentType.Name,
                createdAt = x.CreatedAt,
                createdBy = x.CreatedBy,
                updatedAt = x.UpdatedAt,
                updatedBy = x.UpdatedBy
            }).OrderByDescending(x=>x.id).ToList();

            return rep;
        }
    }
}
