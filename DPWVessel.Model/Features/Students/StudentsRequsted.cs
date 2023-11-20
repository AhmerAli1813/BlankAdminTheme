using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Collections.Generic;
using System.Linq;

namespace DPWVessel.Model.Features.Students
{
    public class GetAllEquipmentTypeRequsted : IRequest<StudentsRespone>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RollNo { get; set; }
        public int Age { get; set; }

    }
    public class StudentsRespone : Response
    {
        public List<StudentsList> StudentsLists { get; set; }
    }
    public class StudentsList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RollNo { get; set; }
        public int Age { get; set; }

    }
    public class StudentsListRequstedHandel : IRequestHandler<GetAllEquipmentTypeRequsted, StudentsRespone>
    {
        private dpw_VesselEntities _dbcontext;
        public StudentsListRequstedHandel(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public StudentsRespone Execute(GetAllEquipmentTypeRequsted request)
        {
            StudentsRespone rep = new StudentsRespone();
            var row = _dbcontext.Students.ToList();


            if (request.Id > 0)
            {
                row = row.Where(x => x.Sid == request.Id).ToList();
            }
            if (request.Name != "" && request.Name != null)
            {
                row = row.Where(x => x.SName == request.Name).ToList();
            }
            if (request.RollNo > 0)
            {
                row = row.Where(x => x.RollNo == request.RollNo).ToList();
            }
            if (request.Age > 0)
            {
                row = row.Where(x => x.Age == request.Age).ToList();
            }

            rep.StudentsLists = row.Select(x => new StudentsList
            {
                Id = x.Sid,
                Name = x.SName,
                RollNo = x.RollNo,
                Age = x.Age
            }).OrderByDescending(x => x.Id).ToList();

            return rep;
        }
    }
}
