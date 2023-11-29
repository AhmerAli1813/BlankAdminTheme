using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Linq;

namespace DPWVessel.Model.Features.UploadFiles
{
    public class UploadFilesInformationUpdateRequest : IRequest<UploadFilesInformationUpdateResponse>
    {
        public UploadFilesData data { get; set; } = new UploadFilesData();
    }
    public class UploadFilesInformationUpdateResponse : Response
    {
        public bool IsTure { get; set; }
        public string message { get; set; }
    }
    public class UploadFilesData
    {
        public int id { get; set; }
        public string filename { get; set; }
        public string file { get; set; }
        public string updatedBy { get; set; }
    }
    public class UploadFilesInformationUpdateRequestHandel : IRequestHandler<UploadFilesInformationUpdateRequest, UploadFilesInformationUpdateResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public UploadFilesInformationUpdateRequestHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public UploadFilesInformationUpdateResponse Execute(UploadFilesInformationUpdateRequest request)
        {
            UploadFilesInformationUpdateResponse rep = new UploadFilesInformationUpdateResponse();
            //var NameValidation = _dbcontext.UploadFiles.Where(x => x.Filename == request.data.filename).Count();
            //if (NameValidation <= 0)
            //{

            var req = _dbcontext.UploadFiles.FirstOrDefault(x => x.Id == request.data.id);

            req.Filename = request.data.filename;
            req.File = request.data.file;
            req.UpdatedAt = DateTime.Now;
            req.UpdatedBy = request.data.updatedBy;
            _dbcontext.SaveChanges();
            rep.IsTure = true;
            rep.message = "Save";
            return rep;
            //}
            //else
            //{
            //    rep.IsTure = false;
            //    rep.message = $"UploadFiles {request.data.name} already taken";
            //    return rep;
            //}

        }
    }
}

