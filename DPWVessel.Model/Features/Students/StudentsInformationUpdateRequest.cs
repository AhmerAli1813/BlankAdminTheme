using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System.Linq;

namespace DPWVessel.Model.Features.Students
{
    public class EquipmentTypeInformationUpdateRequest : IRequest<StudentsInformationUpdateResponse>
    {
        public StudentData data { get; set; }
    }
    public class StudentsInformationUpdateResponse : Response
    {
        public bool IsTure { get; set; }
        public string message { get; set; }
    }
    public class StudentData
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int rollNo { get; set; }
        public int age { get; set; }
    }
    public class StudentsInformationUpdateRequestHandel : IRequestHandler<EquipmentTypeInformationUpdateRequest, StudentsInformationUpdateResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public StudentsInformationUpdateRequestHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public StudentsInformationUpdateResponse Execute(EquipmentTypeInformationUpdateRequest request)
        {
            StudentsInformationUpdateResponse rep = new StudentsInformationUpdateResponse();
            var rollNoValidation = _dbcontext.Students.Where(x => x.RollNo == request.data.rollNo).Count();
            if (rollNoValidation <= 0)
            {

                var req = _dbcontext.Students.FirstOrDefault(x => x.Sid == request.data.Id);

                req.SName = request.data.name;
                req.RollNo = request.data.rollNo;
                req.Age = request.data.age;
                _dbcontext.SaveChanges();
                rep.IsTure = true;
                rep.message = "Save";
                return rep;
            }
            else
            {
                rep.IsTure = false;
                rep.message = $"RollNo {request.data.rollNo} already taken";
                return rep;
            }

        }
    }
}
