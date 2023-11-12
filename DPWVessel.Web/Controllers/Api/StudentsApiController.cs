using DE.Infrastructure.Concept;
using DE.Infrastructure.Security;
using DPWVessel.Model.Features.Students;

using DPWVessel.Web.Core.Session;
using System;
using System.Web.Http;

namespace DPWVessel.Web.Controllers.Api
{
    [Authorize]
    public class StudentsApiController : BaseApiController
    {
        private readonly ISessionManager _sessionManager;

        public StudentsApiController(IRequestExecutor requestExecutor, ISessionManager sessionManager)
        {
            _requestExecutor = requestExecutor;
            _sessionManager = sessionManager;

        }

        [HttpGet]
        public object GetStudentsList([FromUri] GetAllEquipmentTypeRequsted filtring)
        {
            if (filtring != null)
            {
                var resp2 = _requestExecutor.Execute(filtring);
                return resp2;
            }
            var req = new GetAllEquipmentTypeRequsted();
            var resp = _requestExecutor.Execute(req);
            return resp;
        }


        [HttpPost]
        public object AddStudentRec(AddEquipmentTypeRequsted req)
        {
            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpGet]
        public object getStudentDetails([FromUri] GetEquipmentTypeDetailRequest req)
        {

            var resp = _requestExecutor.Execute(req);
            return resp;
        }

        [HttpPost]
        public object StudentsInformationUpdate(EquipmentTypeInformationUpdateRequest req)
        {
            StudentsInformationUpdateResponse resp = new StudentsInformationUpdateResponse();
            resp = _requestExecutor.Execute(req);
            return resp;
        }
        //Encrypt
        [HttpGet]
        public object Encrypt([FromUri] string Id)
        {
            return Encryption.Encrypt(Id.ToString());
        }
        //Decrypt
        [HttpGet]
        public object Decrypt([FromUri] string Id)
        {
            var x = Convert.ToInt32(Encryption.Decrypt(Id.Replace(" ", "+")));
            return x;
        }

        [HttpGet]
        public object Decryptstring([FromUri] string Id)
        {
            var x = Encryption.Decrypt(Id.Replace(" ", "+"));
            return x;
        }



    }

}