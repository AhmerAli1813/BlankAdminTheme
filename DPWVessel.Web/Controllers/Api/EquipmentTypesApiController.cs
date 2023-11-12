using DE.Infrastructure.Concept;
using DE.Infrastructure.Security;
using DPWVessel.Model.Features.EquipmentTypes;

using DPWVessel.Web.Core.Session;
using System;
using System.Web.Http;

namespace DPWVessel.Web.Controllers.Api
{
    [Authorize]
    public class EquipmentTypesApiController : BaseApiController
    {
        private readonly ISessionManager _sessionManager;

        public EquipmentTypesApiController(IRequestExecutor requestExecutor, ISessionManager sessionManager)
        {
            _requestExecutor = requestExecutor;
            _sessionManager = sessionManager;

        }

        [HttpGet]
        public object GetEquipmentTypesList([FromUri] GetAllEquipmentTypeRequsted filtring)
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
        public object AddEquipmentTypesRec(AddEquipmentsTypesRequsted req)
        {
            req.createdBy = _sessionManager.CurrentUser.FullName;
            req.updatedBy = _sessionManager.CurrentUser.FullName;
            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpGet]
        public object getEquipmentTypesDetails([FromUri] GetEquipmentTypeDetailRequest req)
        {

            var resp = _requestExecutor.Execute(req);
            return resp;
        }

        [HttpPost]
        public object EquipmentTypesInformationUpdate(EquipmentTypeInformationUpdateRequest req)
        {
            req.data.updatedBy = _sessionManager.CurrentUser.FullName;
            EquipmentTypesInformationUpdateResponse resp = new EquipmentTypesInformationUpdateResponse();
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