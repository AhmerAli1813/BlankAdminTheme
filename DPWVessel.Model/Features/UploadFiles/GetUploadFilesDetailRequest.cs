using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DPWVessel.Model.Features.UploadFiles
{
    public class GetUploadFilesDetailRequest : IRequest<GetUplodedFilesDetailResponse>
    {
        public int id { get; set; }

    }
    public class GetUplodedFilesDetailResponse : Response
    {
        public int id { get; set; }
        public string name { get; set; }
        public string file { get; set; }
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }
    }

    public class GetUplodedFilesRequestHandel : IRequestHandler<GetUploadFilesDetailRequest, GetUplodedFilesDetailResponse>
    {
        private dpw_VesselEntities _dbcontext;

        public GetUplodedFilesRequestHandel(dpw_VesselEntities dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public GetUplodedFilesDetailResponse Execute(GetUploadFilesDetailRequest request)
        {
            var req = _dbcontext.UploadFiles.FirstOrDefault(x => x.Id == request.id);
            GetUplodedFilesDetailResponse resp = new GetUplodedFilesDetailResponse();
            if (req != null)
            {

                resp.id = req.Id;
                resp.name = req.Filename;
                resp.file = req.File;
                resp.createdAt = req.CreatedAt;
                resp.createdBy = req.CreatedBy;
                resp.updatedAt = req.UpdatedAt;
                resp.updatedBy = req.UpdatedBy;


            }
            return resp;

        }
    }

}
