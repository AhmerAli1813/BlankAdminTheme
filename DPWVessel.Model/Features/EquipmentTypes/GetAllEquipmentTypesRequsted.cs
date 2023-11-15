using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DPWVessel.Model.Features.EquipmentTypes
{
    public class GetAllEquipmentTypeRequsted : IRequest<EquipmentTypesRespone>
    {
        public int? id { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public GetAllEquipmentTypeRequsted()
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;   
        }



    }
    public class EquipmentTypesRespone : Response
    {
        public List<EquipmentTypesList> EquipmentTypesLists { get; set; }
    }
    public class EquipmentTypesList
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }

    }
    public class EquipmentTypesListRequstedHandel : IRequestHandler<GetAllEquipmentTypeRequsted, EquipmentTypesRespone>
    {
        private dpw_VesselEntities _dbcontext;
        public EquipmentTypesListRequstedHandel(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public EquipmentTypesRespone Execute(GetAllEquipmentTypeRequsted request)
        {
            EquipmentTypesRespone rep = new EquipmentTypesRespone();
            var row = _dbcontext.EquipmentTypes.ToList();


            if (request.id > 0)
            {
                row = row.Where(x => x.Id == request.id).ToList();
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
                row = row.Where(x => x.CreatedAt <= request.endDate).ToList();
            }

            rep.EquipmentTypesLists = row.Select(x => new EquipmentTypesList
            {
                id = x.Id,
                name = x.Name,
                createdAt = x.CreatedAt,
                createdBy = x.CreatedBy,
                updatedAt = x.UpdatedAt,
                updatedBy = x.UpdatedBy
            }).OrderByDescending(x=>x.id).ToList();

            return rep;
        }
    }
}
