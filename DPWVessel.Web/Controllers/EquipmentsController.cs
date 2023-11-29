using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using DPWVessel.Model.Features.Equipments;
using DPWVessel.Model.Features.Shared;
using DPWVessel.Web.Core.Session;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPWVessel.Web.Controllers
{
    /// <summary>
    [Authorize(Roles = "Admin")]
    /// </summary>
    public class EquipmentsController : BaseController
    {
        private readonly IRequestExecutor _requestExecutor;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly dpw_VesselEntities _dbContext;
        private readonly ISessionManager _sessionManager;

        public EquipmentsController(dpw_VesselEntities dbContext, ISessionManager sessionManager, IRequestExecutor requestExecutor)
        {
            _dbContext = dbContext;
            _sessionManager = sessionManager;
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _signInManager = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            _requestExecutor = requestExecutor;

        }
        // GET: EquipmentTypes
        [HttpGet]
        public ActionResult Index() => View();

        public ActionResult Add() => View();
        public ActionResult Edit() => View();
        public ActionResult ToExportExcelEquipment(GetAllEquipmentsRequsted req)
        {
            var resp = _requestExecutor.Execute(req);

            var excelExport = new ExcelServices();
            // Replace ViewModel with the actual type of your equipment
            var data = resp.EquipmentsLists.Select(item => new Dictionary<string, object>
                        {
                            { "Id #", item.id },
                            {"Type Id*" ,item.equipmentTypeId },
                            { "Name*", item.name },
                            { "Type Name", item.equipmentTypeName },
                            { "Created At", item.createdAt },
                            { "Created By", item.createdBy }
                        }).ToList();

            var headers = new[] { "Id #", "Type Id*", "Name*", "Type Name", "Created At", "Created By" };


            var excelBytes = excelExport.ExportToExcel(data, headers);


            string filename = $"EquipmentsList_{DateTime.Now.ToString("dd-MM-yyy")}.xlsx";

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        [HttpGet]
        public ActionResult ExcelTemplate()

        {
            var excelExport = new ExcelServices();

            var headers = new[] { "Name*", "Type Name*" };


            var excelBytes = excelExport.ExportToExcel(headers);


            string filename = $"Temp_Equipments_{DateTime.Now.ToString("dd-MM-yyy")}.xlsx";

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);



        }
        public ActionResult Print(int Id)
        {
            GetEquipmentsPrintRequsted req = new GetEquipmentsPrintRequsted();
            req.id = Id;
            var resp = _requestExecutor.Execute(req);

            return View(resp);
        }
        [HttpGet]
        public ActionResult PrintExportToExcel(int Id)
        {
            GetEquipmentsPrintRequsted req = new GetEquipmentsPrintRequsted();
            req.id = Id;
            var resp = _requestExecutor.Execute(req);

            var data = new Dictionary<string, object>
            {
                { "Id #", resp.id },
                {"Type Id*" ,resp.equipmentTypeId },
                { "Name", resp.name },
                { "Type Name*", resp.equipmentTypeName },
                { "Created At", resp.createdAt },
                { "Created By", resp.createdBy }
            };

            var excelExport = new ExcelServices(); // Create your instance of execlExportServices
            var headers = new[] { "Id #", "Type Id*", "Name*", "Type Name", "Created At", "Created By" };



            var excelBytes = excelExport.ExportToExcel(data, headers);

            string filename = $"EquipmentsList_{DateTime.Now.ToString("dd-MM-yyy")}.xlsx";

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase File)
        {
            string tempFileName = string.Empty;
            string errorMessage = string.Empty;
            byte[] ErrorFile;
            if (File != null && File.ContentLength > 0)
            {
                //ExcelExtension contains array of excel extension types
                //save the uploaded excel file to temp location
                SaveExcelTemp(File, out tempFileName);
                ExcelServices ImportService = new ExcelServices();
                //Here ExpectedHeaders , Here you given header of your excel file which are correct formate in case user change sequence of excel column then given error  
                List<string> ExpectedHeaders = new List<string> { "Name*", "Type Name*" };

                Dictionary<int, Func<ExcelRange, string, bool>> validationMethods = new Dictionary<int, Func<ExcelRange, string, bool>>
                        {
                            { 1, ImportService.ValidateText },             //Dictionary<int, Func<ExcelRange, string, bool>>: This declares a dictionary where keys
                            { 2, ImportService.ValidateText },            //are integers(column indices) and values are functions taking an ExcelRange and a string
                          //{ 3, ValidateEmailAddress },                 //and returning a bool.
                          //{ 4, ValidateNumber }                       //{ 1, ValidateText },: This is an entry in the dictionary where the key is 1(column index)
                                                                       //and the value is the ValidateText function.This is a comment indicating that it associates
                                                                      //column index 1 with the ValidateText function.
                                                                     //Here Your given index of excel column index into int and given validationMethodName: This
                                                                    // is a comment explaining that the code associates Excel column indices with validation method names.
                                                                   // Add more mappings as needed
                        };

                //validate the excel sheet here is excel format is valid then you can Insert Records
                if (ImportService.ValidateExcel(tempFileName, out errorMessage, out ErrorFile, ExpectedHeaders, validationMethods))
                {
                    if (errorMessage == "")
                    {
                        errorMessage = "Save Records  Successfully";
                    }

                    try
                    {
                        using (var stream = System.IO.File.OpenRead(tempFileName))
                        {
                            using (ExcelPackage package = new ExcelPackage(stream))
                            {

                                var workbook = package.Workbook;

                                // Get the names of all sheets in the workbook
                                var sheetNames = workbook.Worksheets.Select(sheet => sheet.Name).ToList();
                                if (sheetNames.Count > 0)
                                {
                                    string FirstSheetName = sheetNames[0];
                                    ExcelWorksheet worksheet = package.Workbook.Worksheets[FirstSheetName];
                                    List<int> invalidRows = new List<int>();

                                    if (worksheet.Dimension != null)
                                    {
                                        int totalRows = worksheet.Dimension.Rows;

                                        for (int i = 2; i <= totalRows; i++)
                                        {
                                            object nameValue = worksheet.Cells[i, 1].Value;
                                            string name = (nameValue == null || string.IsNullOrWhiteSpace(nameValue.ToString())) ? null : nameValue.ToString();

                                            object nameTypeValue = worksheet.Cells[i, 2].Value;
                                            string nameType = (nameTypeValue == null || string.IsNullOrWhiteSpace(nameTypeValue.ToString())) ? null : nameTypeValue.ToString();

                                            int typeId = _dbContext.EquipmentTypes
                                                .Where(x => x.Name == nameType)
                                                .Select(x => x.Id)
                                                .FirstOrDefault();

                                            string userName = _sessionManager.CurrentUser.FullName;

                                            if (typeId > 0 && name != null)
                                            {
                                                AddEquipmentsRequsted req = new AddEquipmentsRequsted
                                                {
                                                    equipmentsTypeId = typeId,
                                                    name = name,
                                                    createdBy = userName,
                                                    updatedBy = userName
                                                };

                                                var resp = _requestExecutor.Execute(req);
                                                errorMessage = resp.message;
                                            }
                                            else
                                            {
                                                invalidRows.Add(i);
                                            }
                                        }

                                        if (invalidRows.Any())
                                        {
                                            byte[] invalidRecordsFile = ImportService.SaveInvalidRecordsToExcel(worksheet, invalidRows);

                                            return Json(new { ErrorByt = invalidRecordsFile, msg2 = "File", msg = "Some records are not inserted", Isture = false });
                                        }
                                    }
                                }
                                else
                                {
                                    return Json(new { ErrorByt = ErrorFile, msg2 = "File", msg = "No Sheet takings ", Isture = false });
                                }


                            }
                        }
                        DeleteTempFile(tempFileName);
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception appropriately
                        // You might want to log the exception or return an error response
                        return Json(new { ErrorByt = ErrorFile, msg2 = "File", msg = "Exception:" + ex.Message, Isture = true });
                    }
                    DeleteTempFile(tempFileName);

                    return Json(new { ErrorByt = ErrorFile, msg2 = "File", msg = errorMessage, Isture = true });

                    //spreadsheet is valid, show success message or any logic here
                }
                else
                {
                    //set error message to shown in front end

                    DeleteTempFile(tempFileName);
                    return Json(new { ErrorByt = ErrorFile, msg2 = "File", msg = errorMessage, Isture = false });

                }

            }
            else
            {
                //file is not uploaded, show error message
                return Json(new { msg = "file is not uploaded", Isture = false });
            }


        }
        [HttpPost]
        public ActionResult uploadAltra(HttpPostedFileBase File)
        {
            string tempFileName = string.Empty;
            string errorMessage = string.Empty;

            if (File != null && File.ContentLength > 0)
            {
                //ExcelExtension contains array of excel extension types
                //save the uploaded excel file to temp location
                SaveExcelTemp(File, out tempFileName);
                ExcelServices ImportService = new ExcelServices();

                try
                {
                    using (var stream = System.IO.File.OpenRead(tempFileName))
                    {
                        using (ExcelPackage package = new ExcelPackage(stream))
                        {

                            var workbook = package.Workbook;

                            // Get the names of all sheets in the workbook
                            var sheetNames = workbook.Worksheets.Select(sheet => sheet.Name).ToList();
                            if (sheetNames.Count > 0)
                            {
                                string FirstSheetName = sheetNames[0];
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[FirstSheetName];
                                List<int> invalidRows = new List<int>();

                                // Check if the sheet is empty
                                if (worksheet.Dimension == null || worksheet.Dimension.Rows < 2 || worksheet.Dimension.Columns < 2)
                                {
                                    return Json(new { msg = "Empty or incomplete sheet uploaded", Isture = false });
                                }

                                // Check header sequence
                                string expectedHeader1 = "Name*";
                                string expectedHeader2 = "Type Name*";
                                if (worksheet.Cells[1, 1].Text != expectedHeader1 || worksheet.Cells[1, 2].Text != expectedHeader2)
                                {
                                    return Json(new { msg = "Invalid header sequence.", Isture = false });
                                }

                                int totalRows = worksheet.Dimension.Rows;

                                for (int i = 2; i <= totalRows; i++)
                                {
                                    object nameValue = worksheet.Cells[i, 1].Value;
                                    string name = (nameValue == null || string.IsNullOrWhiteSpace(nameValue.ToString())) ? null : nameValue.ToString();

                                    object nameTypeValue = worksheet.Cells[i, 2].Value;
                                    string nameType = (nameTypeValue == null || string.IsNullOrWhiteSpace(nameTypeValue.ToString())) ? null : nameTypeValue.ToString();

                                    int typeId = _dbContext.EquipmentTypes
                                        .Where(x => x.Name == nameType)
                                        .Select(x => x.Id)
                                        .FirstOrDefault();

                                    string userName = _sessionManager.CurrentUser.FullName;

                                    if (typeId > 0 && name != null)
                                    {
                                        AddEquipmentsRequsted req = new AddEquipmentsRequsted
                                        {
                                            equipmentsTypeId = typeId,
                                            name = name,
                                            createdBy = userName,
                                            updatedBy = userName
                                        };

                                        var resp = _requestExecutor.Execute(req);
                                        errorMessage = resp.message;
                                    }
                                    else
                                    {
                                        invalidRows.Add(i);
                                    }
                                }

                                if (invalidRows.Any())
                                {
                                    byte[] invalidRecordsFile = ImportService.SaveInvalidRecordsToExcel(worksheet, invalidRows);

                                    return Json(new { ErrorByt = invalidRecordsFile, msg2 = "File", msg = "Some records are not inserted", Isture = false });
                                }
                            }
                            else
                            {
                                return Json(new { msg = "Empty Sheet Uploaded", Isture = false });
                            }
                        }
                    }
                    DeleteTempFile(tempFileName);
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately
                    // You might want to log the exception or return an error response
                    return Json(new { msg = "Exception:" + ex.Message, Isture = false });
                }
                DeleteTempFile(tempFileName);

                return Json(new { msg2 = "File", msg = errorMessage, Isture = true });
            }
            else
            {
                //file is not uploaded, show error message
                return Json(new { msg = "file is not uploaded", Isture = false });
            }
        }
        private void DeleteTempFile(string tempFileName)
        {
            try
            {
                // Check if the file exists before attempting to delete
                if (System.IO.File.Exists(tempFileName))
                {
                    System.IO.File.Delete(tempFileName);
                    Console.WriteLine($"Temporary file {tempFileName} deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Temporary file {tempFileName} does not exist.");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                // You might want to log the exception or return an error response
                Console.WriteLine($"Error deleting temporary file: {ex.Message}");
            }
        }
        private void SaveExcelTemp(HttpPostedFileBase file, out string tempFileName)
        {
            // Implementation of the method to save the Excel file to a temporary location
            // You can use libraries like EPPlus or NPOI for working with Excel files
            // Example: Save the file to a folder on the server
            tempFileName = Path.Combine(Server.MapPath("~/Content/svgs"), file.FileName);
            file.SaveAs(tempFileName);
        }
    }
}