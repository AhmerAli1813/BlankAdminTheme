using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using DPWVessel.Model.Features.UploadFiles;
using DPWVessel.Web.Core.Session;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DPWVessel.Web.Controllers
{
    /// <summary>
    [Authorize(Roles = "Admin")]
    /// </summary>
    public class UploadFilesController : BaseController
    {
        private readonly IRequestExecutor _requestExecutor;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly dpw_VesselEntities _dbContext;
        private readonly ISessionManager _sessionManager;

        public UploadFilesController(dpw_VesselEntities dbContext, ISessionManager sessionManager, IRequestExecutor requestExecutor)
        {
            _dbContext = dbContext;
            _sessionManager = sessionManager;
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _signInManager = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            _requestExecutor = requestExecutor;

        }
        // GET: UploadFiles
        public ActionResult Index() => View();
        public ActionResult Add() => View();
        public ActionResult Edit() => View();
        [HttpPost]
        public ActionResult AddUploadFilesRec(List<HttpPostedFileBase> files)
        {
            var resp = new object();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        // Save the file to the server
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/content/UploadFiles"), fileName);
                        file.SaveAs(filePath);

                        // Save file information to the database
                        AddUploadFilesRequsted req = new AddUploadFilesRequsted();
                        req.filename = fileName;
                        req.uploadfile = "~/content/UploadFiles/" + fileName;
                        req.createdBy = _sessionManager.CurrentUser.FullName;
                        req.updatedBy = _sessionManager.CurrentUser.FullName;
                        resp = _requestExecutor.Execute(req);
                    }
                }
            }

            return Json(resp);
        }

        public ActionResult Download(int Id)
        {
            GetUploadFilesDetailRequest req = new GetUploadFilesDetailRequest();
            req.id = Id;
            var resp = _requestExecutor.Execute(req);

            if (resp != null)
            {
                // Resolve the virtual path to a physical path
                string filePath = Server.MapPath(resp.file);

                if (System.IO.File.Exists(filePath))
                {
                    // Determine content type based on file extension
                    string contentType;
                    switch (Path.GetExtension(filePath).ToLowerInvariant())
                    {
                        // Add cases for other file types as needed
                        case ".jpg":
                            contentType = "image/jpeg";
                            break;
                        case ".png":
                            contentType = "image/png";
                            break;
                        case ".xlsx":
                            contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                        case ".docx":
                            contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        case ".pdf":
                            contentType = "application/pdf";
                            break;
                        default:
                            contentType = "application/octet-stream"; // Default content type for unknown file types
                            break;
                    }

                    // Set the content-disposition header to force the browser to prompt the user to download the file
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + resp.name);

                    // Return the file
                    return File(filePath, contentType);
                }
            }

            // Handle the case when the file is not found
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Update(int id, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                // Get the details of the existing file
                GetUploadFilesDetailRequest detail = new GetUploadFilesDetailRequest();
                detail.id = id;  // Assuming there's an id property in GetUploadFilesDetailRequest
                var existingFileDetails = _requestExecutor.Execute(detail);

                try
                {
                    // Save the new file

                    var fileName = Path.GetFileName(file.FileName) + "_" + DateTime.Now;
                    // Update file information in the database
                    UploadFilesInformationUpdateRequest req = new UploadFilesInformationUpdateRequest();
                    req.data.updatedBy = _sessionManager.CurrentUser.FullName;
                    req.data.id = id;
                    req.data.filename = file.FileName; // Assuming you want to update the file name
                    req.data.file = "~/content/UploadFiles/" + fileName;   // Assuming you want to update the file path
                    var resp = _requestExecutor.Execute(req);

                    SaveUploadedFile(file, existingFileDetails.file);

                    return Json(new { IsTrue = resp.IsTure, msg = resp.message });
                }
                catch (Exception ex)
                {
                    // Log the exception for troubleshooting


                    return Json(new { IsTrue = false, msg = "An error occurred during file update. Please try again later.", exmsg = ex.Message });
                }
            }

            return Json(new { IsTrue = false, msg = "Invalid file or file content length is zero." });
        }


        private void SaveUploadedFile(HttpPostedFileBase file, string existingFilePath)
        {
            // Map the existing virtual path to a physical path
            var existingPhysicalPath = Server.MapPath(existingFilePath);

            // Delete the existing file
            if (!string.IsNullOrEmpty(existingPhysicalPath) && System.IO.File.Exists(existingPhysicalPath))
            {
                System.IO.File.Delete(existingPhysicalPath);
            }

            // Save the new file to the server and return the new file path
            var fileName = Path.GetFileName(file.FileName);
            var newFilePath = Path.Combine(Server.MapPath("~/content/UploadFiles"), fileName);
            file.SaveAs(newFilePath);
        }

        [HttpPost]
        public ActionResult DeleteUp(int Id)
        {
            try
            {
                var fileToRemove = _dbContext.UploadFiles.Find(Id);

                if (fileToRemove != null)
                {
                    _dbContext.UploadFiles.Remove(fileToRemove);
                    _dbContext.SaveChanges();
                    var existingPhysicalPath = Server.MapPath(fileToRemove.File);

                    // Delete the existing file
                    if (!string.IsNullOrEmpty(existingPhysicalPath) && System.IO.File.Exists(existingPhysicalPath))
                    {
                        System.IO.File.Delete(existingPhysicalPath);
                    }
                    return Json(new { IsTure = true, msg = "Successfully Deleted" });


                }
            }
            catch
            {
                return Json(new { IsTure = false, msg = "Getting something Wrong" });
            }
            return Json(new { IsTure = false, msg = "Error Message" });
        }

    }
}