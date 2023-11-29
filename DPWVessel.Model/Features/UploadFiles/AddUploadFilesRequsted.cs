using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;

namespace DPWVessel.Model.Features.UploadFiles
{
    public class AddUploadFilesRequsted : IRequest<AddUploadFilesResponse>
    {
        public int id { get; set; }
        public string filename { get; set; }
        public string uploadfile { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
    }
    public class AddUploadFilesResponse : Response
    {
        public bool IsTure { get; set; }
        public string msg { get; set; }
    }

    public class AddUploadFilesRequstedRequstedHandel : IRequestHandler<AddUploadFilesRequsted, AddUploadFilesResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public AddUploadFilesRequstedRequstedHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public AddUploadFilesResponse Execute(AddUploadFilesRequsted request)
        {
            AddUploadFilesResponse rep = new AddUploadFilesResponse();
            // var NameValidation = _dbcontext.EquipmentTypes.Where(x => x.Name == request.filename).Count();
            //if (NameValidation <= 0)
            //{
            try
            {

                UploadFile model = new UploadFile
                {
                    Filename = request.filename,
                    File = request.uploadfile,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.createdBy,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = request.updatedBy

                };

                _dbcontext.UploadFiles.Add(model);
                _dbcontext.SaveChanges();
                rep.IsTure = true;
                rep.msg = "Save";
            }
            catch (Exception ex)
            {
                rep.msg = ex.Message;
                rep.IsTure = false;
            }

            return rep;
            //}
            //else
            //{
            //    rep.IsTure = false;
            //    rep.msg = $"Equipment Types : {request.filename} already taken";
            //    return rep;
            //}
        }
    }
}
