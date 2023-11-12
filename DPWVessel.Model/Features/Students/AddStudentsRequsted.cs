using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Linq;

namespace DPWVessel.Model.Features.Students
{
    public class AddEquipmentTypeRequsted : IRequest<AddStudentsResponse>
    {
        public string name { get; set; }
        public int rollNo { get; set; }
        public int age { get; set; }
    }
    public class AddStudentsResponse : Response
    {
        public bool IsTure { get; set; }
        public string message { get; set; }
    }

    public class AddStudentsRequstedHandel : IRequestHandler<AddEquipmentTypeRequsted, AddStudentsResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public AddStudentsRequstedHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public AddStudentsResponse Execute(AddEquipmentTypeRequsted request)
        {
            AddStudentsResponse rep = new AddStudentsResponse();
            var rollNoValidation = _dbcontext.Students.Where(x => x.RollNo == request.rollNo).Count();
            if (rollNoValidation <= 0)
            {

                Student student = new Student
                {
                    SName = request.name,
                    Age = request.age,
                    RollNo = request.rollNo
                };

                _dbcontext.Students.Add(student);
                _dbcontext.SaveChanges();
                rep.IsTure = true;
                rep.message = "Save";
                return rep;
            }
            else
            {
                rep.IsTure = false;
                rep.message = $"RollNo {request.rollNo} already taken";
                return rep;
            }




        }
    }
}
