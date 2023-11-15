using DE.Infrastructure.Concept;
using DE.Infrastructure.Security;
using DPWVessel.Model.Features.Equipments;

using DPWVessel.Model.Features.EquipmentTypes;
using DPWVessel.Web.Core.Session;
using System;
using System.Web.Http;

namespace DPWVessel.Web.Controllers.Api
{
    [Authorize]
    public class EquipmentsApiController : BaseApiController
    {
        private readonly ISessionManager _sessionManager;

        public EquipmentsApiController(IRequestExecutor requestExecutor, ISessionManager sessionManager)
        {
            _requestExecutor = requestExecutor;
            _sessionManager = sessionManager;

        }

        [HttpGet]
        public object GetEquipmentsList([FromUri] GetAllEquipmentsRequsted req)
        {
            var resp = _requestExecutor.Execute(req);
            return resp;
        }

        [HttpPost]
        public object AddEquipmentsRec(AddEquipmentsRequsted req)
        {
            req.createdBy = _sessionManager.CurrentUser.FullName;
            req.updatedBy = _sessionManager.CurrentUser.FullName;
            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpGet]
        public object getEquipmentsDetails([FromUri] GetEquipmentsDetailRequest req)
        {

            var resp = _requestExecutor.Execute(req);
            return resp;
        }

        [HttpPost]
        public object EquipmentsInformationUpdate(UpdateEquipmentsInformationRequest req)
        {
            req.data.updatedBy = _sessionManager.CurrentUser.FullName;
            UpdateEquipmentsInformationResponse resp = new UpdateEquipmentsInformationResponse();
            resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpGet]
        public object GetAllEquipmentsTypesOpt()
        {
            var req = new GetEquipmentTypesOptRequsted();
            var resp = _requestExecutor.Execute(req);
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