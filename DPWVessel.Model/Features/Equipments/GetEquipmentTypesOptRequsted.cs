using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Collections.Generic;
using System.Linq;

namespace DPWVessel.Model.Features.EquipmentTypes
{
    public class GetEquipmentTypesOptRequsted : IRequest<GetEquipmentTypesOptRespone>
    {
        public int id { get; set; }
        public string name { get; set; }

    }
    public class GetEquipmentTypesOptRespone : Response
    {
        public List<EquipmentTypesOpt> EquipmentTypesOpt { get; set; }
    }
    public class EquipmentTypesOpt
    {
        public int id { get; set; }
        public string name { get; set; }

    }
    public class GetEquipmentTypesOptRequstedHandel : IRequestHandler<GetEquipmentTypesOptRequsted, GetEquipmentTypesOptRespone>
    {
        private dpw_VesselEntities _dbcontext;
        public GetEquipmentTypesOptRequstedHandel(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public GetEquipmentTypesOptRespone Execute(GetEquipmentTypesOptRequsted request)
        {
            GetEquipmentTypesOptRespone rep = new GetEquipmentTypesOptRespone();
            var row = _dbcontext.EquipmentTypes.ToList();


            rep.EquipmentTypesOpt = row.Select(x => new EquipmentTypesOpt
            {
                id = x.Id,
                name = x.Name,
            }).OrderByDescending(x => x.id).ToList();

            return rep;
        }
    }
}
