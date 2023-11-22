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

            var excelExport = new ExcelExportServices();
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
            var excelExport = new ExcelExportServices();

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

            var excelExport = new ExcelExportServices(); // Create your instance of execlExportServices
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
                List<string> expectedHeaders = new List<string> { "Name*", "Type Name*" };
                //validate the excel sheet
                if (ValidateExcel(tempFileName, out errorMessage, out ErrorFile, expectedHeaders))
                {
                    if (errorMessage == "")
                    {
                        errorMessage = "Save Records  Successfully";
                    }
                    //save the data
                    //SaveExcelDataToDatabase(tempFileName);
                    try
                    {
                        using (var stream = System.IO.File.OpenRead(tempFileName))
                        {
                            using (ExcelPackage package = new ExcelPackage(stream))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
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

                                            _requestExecutor.Execute(req);
                                        }
                                        else
                                        {
                                            invalidRows.Add(i);
                                        }
                                    }

                                    if (invalidRows.Any())
                                    {
                                        byte[] invalidRecordsFile = SaveInvalidRecordsToExcel(worksheet, invalidRows);

                                        return Json(new { ErrorByt = invalidRecordsFile, msg2 = "File", msg = "Some records are not inserted", Isture = false });
                                    }
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

        private byte[] SaveInvalidRecordsToExcel(ExcelWorksheet worksheet, List<int> invalidRows)
        {
            using (var package = new ExcelPackage())
            {
                var invalidSheet = package.Workbook.Worksheets.Add("InvalidRecords");

                // Copy headers from the original sheet to the new sheet
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    invalidSheet.Cells[1, col].Value = worksheet.Cells[1, col].Value;
                }

                // Mark invalid rows with red color and add a comment
                foreach (int row in invalidRows)
                {
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        var sourceCell = worksheet.Cells[row, col];
                        var invalidCell = invalidSheet.Cells[row, col];

                        invalidCell.Value = sourceCell.Value;

                        var fill = invalidCell.Style.Fill;
                        fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                        invalidCell.AddComment("Record not inserted", "");
                    }
                }

                return package.GetAsByteArray();
            }
        }

        private void SaveExcelDataToDatabase(string tempFileName)
        {
            try
            {
                using (var stream = System.IO.File.OpenRead(tempFileName))
                {
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
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

                                    _requestExecutor.Execute(req);
                                }
                                else
                                {
                                    invalidRows.Add(i);
                                }
                            }

                            if (invalidRows.Any())
                            {
                                byte[] invalidRecordsFile = SaveInvalidRecordsToExcel(worksheet, invalidRows);
                                // Do something with the invalidRecordsFile, like saving it to disk or sending it as a response
                                TempData["InvlidRecors"] = invalidRecordsFile;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                // You might want to log the exception or return an error response
                Console.WriteLine(ex.Message);
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

        private List<string> GetColumnHeaders(ExcelWorksheet worksheet)
        {
            List<string> columnHeaders = new List<string>();

            int totalColumns = worksheet.Dimension == null ? -1 : worksheet.Dimension.End.Column;

            for (int i = 1; i <= totalColumns; i++)
            {
                var cell = worksheet.Cells[1, i];

                if (cell.Value != null)
                {
                    // Add the header value to the list
                    columnHeaders.Add(cell.Value.ToString());
                }
            }

            return columnHeaders;
        }

        private bool ValidateColumns(ExcelWorksheet worksheet, int totalColumns)
        {
            bool result = true;

            // Get the column headers dynamically
            List<string> listColumnHeaders = GetColumnHeaders(worksheet);

            // Check if the number of columns in the worksheet matches the expected number of columns
            if (totalColumns != listColumnHeaders.Count)
            {
                result = false;
                // Add an appropriate error message
                // You might want to customize this message based on your requirements
                // For example, you can specify which headers are missing or extraneous
                // error = "Spread sheet column header value mismatch.";
            }

            return result;
        }

        private bool ValidateExcel(string tempFileName, out string errorMessage, out byte[] ErrorFile, List<string> expectedHeaders)
        {
            bool result = true;
            string error = string.Empty;
            string filePath = GetTempFilePath(tempFileName);
            FileInfo fileInfo = new FileInfo(filePath);
            ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.First();
            int totalRows = worksheet.Dimension == null ? -1 : worksheet.Dimension.End.Row; //worksheet total rows
            int totalCols = worksheet.Dimension == null ? -1 : worksheet.Dimension.End.Column; // total columns
            if (worksheet.Dimension == null)
            {
                // Handle the case where the worksheet is empty
                result = false;
                error = "Empty worksheet uploaded.";
            }
            else
            {

                // Check if the spreadsheet has rows (empty spreadsheet uploaded)
                if (totalRows == 0 || totalRows == -1)
                {
                    result = false;
                    error = "Empty spreadsheet uploaded.";
                }
                // Check if there is only one row (header row) and no data rows
                else if (totalRows == 1 && worksheet.Cells[1, 1, 1, totalCols].All(c => c.Text == null || string.IsNullOrWhiteSpace(c.Text)))
                {
                    result = false;
                    error = "Spreadsheet contains only a header row without any data.";
                }
                // Check if there are more than or equal to 2 rows
                else if (totalRows < 2)
                {
                    result = false;
                    error = "Spreadsheet does not contain enough data rows.";
                }
                else if (CheckExcelHeaderFormat(worksheet, expectedHeaders)==false)
                {
                    result = false;
                    error = "Spread sheet column header indexing mismatch.";
                }
                // Check if the total columns equal the headers defined (less columns)
                else if (totalCols > 0 && !ValidateColumns(worksheet, totalCols))
                {
                    result = false;
                    error = "Spread sheet column header value mismatch.";
                }

                if (result)
                {
                    // Validate header columns
                    result &= ValidateColumns(worksheet, totalCols);

                    if (result)
                    {
                        // Validate data rows, skip the header row (data rows start from 2)
                        result &= ValidateRows(worksheet, totalRows, totalCols);
                    }

                    if (!result)
                    {
                        error = "There are some errors in the uploaded file. Please correct them and upload again.";
                    }
                }

            }

            errorMessage = error;
            // worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            ErrorFile = excelPackage.GetAsByteArray();
            // excelPackage.Save();

            return result;
        }


        private string GetTempFilePath(string tempFileName)
        {
            // Assuming the tempFileName is a relative path, combine it with the application's temporary folder
            return Path.Combine(Server.MapPath("~/Content/svgs"), tempFileName);
        }


        private bool SetError(ExcelRange cell, string errorComment)
        {
            var fill = cell.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
            cell.AddComment(errorComment, "");

            return false;
        }

        private List<string> GetColumnHeaders(ExcelWorksheet worksheet, int totalColumns)
        {
            List<string> columnHeaders = new List<string>();

            for (int i = 1; i <= totalColumns; i++)
            {
                var cell = worksheet.Cells[1, i];

                if (cell.Value != null)
                {
                    // Add the header value to the list
                    columnHeaders.Add(cell.Value.ToString());
                }
            }

            return columnHeaders;
        }

        private bool ValidateHeaderColumns(ExcelWorksheet worksheet, int totalColumns)
        {
            bool result = true;

            // Get the column headers dynamically
            List<string> listColumnHeaders = GetColumnHeaders(worksheet, totalColumns);

            for (int i = 1; i <= totalColumns; i++)
            {
                var cell = worksheet.Cells[1, i]; //header columns are in the first row

                if (cell.Value != null)
                {
                    //column header has a value
                    if (!listColumnHeaders.Contains(cell.Value.ToString()))
                    {
                        result &= SetError(cell, "Invalid header. Please correct.");
                    }
                }
                else
                {
                    //empty header
                    result &= SetError(cell, "Empty header. Remove the column.");
                }
            }

            return result;
        }

        public bool CheckExcelHeaderFormat(ExcelWorksheet worksheet , List<string> expectedHeaders)
        {
            bool result = true;

        

            List<string> actualHeaders = GetColumnHeaders(worksheet);

            for (int i = 0; i < expectedHeaders.Count; i++)
            {
                if (i >= actualHeaders.Count || !string.Equals(expectedHeaders[i], actualHeaders[i], StringComparison.OrdinalIgnoreCase))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
            private bool ValidateRows(ExcelWorksheet worksheet, int totalRows, int totalCols)
        {
            bool result = true;

            for (int i = 2; i <= totalRows; i++) //data rows start from 2`
            {
                for (int j = 1; j <= totalCols; j++)
                {
                    var cell = worksheet.Cells[i, j];

                    switch (j)
                    {


                        // name
                        case 1:
                            {
                                result &= ValidateText(cell, "Name*");
                                break;
                            }
                        //Type  name
                        case 2:
                            {
                                result &= ValidateText(cell, "Type Name*");
                                break;
                            }

                    }
                }
            }

            return result;
        }



        private bool ValidateText(ExcelRange cell, string columnName)
        {
            bool result = true;
            string error = string.Format("{0} is empty", columnName);

            if (cell.Value != null)
            {
                //check if cell value has a value
                if (string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    result = SetError(cell, error);
                }
            }
            else
            {
                result = SetError(cell, error);
            }

            return result;
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

        [HttpGet]
        public ActionResult ImportUserData()
        {

            return View();
        }


    }
}