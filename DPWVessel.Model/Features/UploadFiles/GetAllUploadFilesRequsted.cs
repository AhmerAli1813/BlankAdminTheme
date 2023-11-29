using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DPWVessel.Model.Features.UploadFiles
{
    public class GetAllUploadFilesRequsted : IRequest<GetAllUploadFilesRespone>
    {
        public int? id { get; set; }
        public string name { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public GetAllUploadFilesRequsted()
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;
        }



    }
    public class GetAllUploadFilesRespone : Response
    {
        public List<GetAllUploadFilesList> GetAllUploadFilesLists { get; set; }
    }
    public class GetAllUploadFilesList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string file { get; set; }
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }

    }
    public class GetAllUploadFilesListRequstedHandel : IRequestHandler<GetAllUploadFilesRequsted, GetAllUploadFilesRespone>
    {
        private dpw_VesselEntities _dbcontext;
        public GetAllUploadFilesListRequstedHandel(dpw_VesselEntities _context)
        {
            _dbcontext = _context;
        }

        public GetAllUploadFilesRespone Execute(GetAllUploadFilesRequsted request)
        {
            GetAllUploadFilesRespone rep = new GetAllUploadFilesRespone();
            var row = _dbcontext.UploadFiles.ToList();


            if (request.id > 0)
            {
                row = row.Where(x => x.Id == request.id).ToList();
            }
            if (request.name != "" && request.name != null)
            {
                row = row.Where(x => x.Filename == request.name).ToList();
            }
            //if (request.file != "" && request.file != null)
            //{
            //    row = row.Where(x => x.File == request.file).ToList();
            //}
            if (request.startDate != DateTime.MinValue)
            {
                row = row.Where(x => x.CreatedAt >= request.startDate).ToList();
            }
            if (request.endDate != DateTime.MinValue)
            {
                request.endDate = request.endDate.AddDays(1);
                row = row.Where(x => x.CreatedAt <= request.endDate).ToList();
            }

            rep.GetAllUploadFilesLists = row.Select(x => new GetAllUploadFilesList
            {
                id = x.Id,
                name = x.Filename,
                file = x.File,
                createdAt = x.CreatedAt,
                createdBy = x.CreatedBy,
                updatedAt = x.UpdatedAt,
                updatedBy = x.UpdatedBy
            }).OrderByDescending(x => x.id).ToList();

            return rep;
        }
    }
}
