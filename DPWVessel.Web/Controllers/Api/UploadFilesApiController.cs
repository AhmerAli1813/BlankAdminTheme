using DE.Infrastructure.Concept;
using DE.Infrastructure.Security;
using DPWVessel.Model.Features.UploadFiles;
using DPWVessel.Web.Core.Session;
using System;
using System.IO;
using System.Web;
using System.Web.Http;

namespace DPWVessel.Web.Controllers.Api
{
    [Authorize]
    public class UploadFilesApiController : BaseApiController
    {
        private readonly ISessionManager _sessionManager;

        public UploadFilesApiController(IRequestExecutor requestExecutor, ISessionManager sessionManager)
        {
            _requestExecutor = requestExecutor;
            _sessionManager = sessionManager;

        }

        [HttpGet]
        public object GetUploadFilesList([FromUri] GetAllUploadFilesRequsted req)
        {

            var resp = _requestExecutor.Execute(req);
            return resp;
        }


        [HttpPost]
        public IHttpActionResult Upload()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        // Save file to server folder
                        var filePath = HttpContext.Current.Server.MapPath("~/content/upload/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);

                        // Save file information to database
                        AddUploadFilesRequsted req = new AddUploadFilesRequsted();
                        req.filename = postedFile.FileName;
                        req.uploadfile = filePath;
                        req.createdBy = _sessionManager.CurrentUser.FullName;
                        req.updatedBy = _sessionManager.CurrentUser.FullName;
                        var resp = _requestExecutor.Execute(req);

                        // Handle the response if needed
                    }

                    return Ok("Files uploaded successfully.");
                }
                else
                {
                    return BadRequest("No files received.");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        public object getUploadFilesDetails([FromUri] GetUploadFilesDetailRequest req)
        {

            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpPost]
        public object update2(int Id, HttpPostedFileBase file)
        {
            return Json(new { id = Id, File = file });
        }
        [HttpPost]
        public object update(int Id, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                // Get the details of the existing file
                GetUploadFilesDetailRequest detail = new GetUploadFilesDetailRequest();
                detail.id = Id;  // Assuming there's an id property in GetUploadFilesDetailRequest
                var existingFileDetails = _requestExecutor.Execute(detail);

                // Delete the existing file
                DeleteFile(existingFileDetails.file);

                // Save the new file
                var newFilePath = SaveUploadedFile(file);

                // Update file information in the database
                UploadFilesInformationUpdateRequest req = new UploadFilesInformationUpdateRequest();
                req.data.updatedBy = _sessionManager.CurrentUser.FullName;
                req.data.id = Id;
                req.data.filename = file.FileName; // Assuming you want to update the file name
                req.data.file = newFilePath;   // Assuming you want to update the file path
                var resp = _requestExecutor.Execute(req);

                return resp;
            }

            return Json(new { isTrue = false });
        }

        private string SaveUploadedFile(HttpPostedFileBase file)
        {
            // Save the file to the server and return the new file path
            var fileName = Path.GetFileName(file.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/content/svgs/" + fileName);
            file.SaveAs(filePath);

            return filePath;
        }

        private void DeleteFile(string filePath)
        {
            // Delete the file from the server
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
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