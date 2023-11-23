using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DPWVessel.Model.Features.Shared
{

    public class ExcelValidator
    {
        public void SaveExcelTemp(HttpPostedFileBase file, out string tempFileName)
        {
            // Implementation of the method to save the Excel file to a temporary location
            // You can use libraries like EPPlus or NPOI for working with Excel files
            // Example: Save the file to a folder on the server
            tempFileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/svgs"), file.FileName);
            file.SaveAs(tempFileName);
        }

        public bool ValidateExcel(string tempFileName, out string errorMessage, out byte[] errorFile, Func<int, string> validationColumnProvider)
        {
            bool result = true;
            string error = string.Empty;
            string filePath = GetTempFilePath(tempFileName);
            FileInfo fileInfo = new FileInfo(filePath);
            ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.First();
            int totalRows = worksheet.Dimension == null ? -1 : worksheet.Dimension.End.Row;
            int totalCols = worksheet.Dimension == null ? -1 : worksheet.Dimension.End.Column;

            if (worksheet.Dimension == null)
            {
                result = false;
                error = "Empty worksheet uploaded.";
            }
            else
            {
                if (totalRows == 0 || totalRows == -1)
                {
                    result = false;
                    error = "Empty spreadsheet uploaded.";
                }
                else if (totalRows == 1 && worksheet.Cells[1, 1, 1, totalCols].All(c => c.Text == null || string.IsNullOrWhiteSpace(c.Text)))
                {
                    result = false;
                    error = "Spreadsheet contains only a header row without any data.";
                }
                else if (totalRows < 2)
                {
                    result = false;
                    error = "Spreadsheet does not contain enough data rows.";
                }
                else if (totalCols > 0 && !ValidateColumns(worksheet, totalCols))
                {
                    result = false;
                    error = "Spread sheet column header value mismatch.";
                }

                if (result)
                {
                    result &= ValidateHeaderColumns(worksheet, totalCols);

                    if (result)
                    {
                        result &= ValidateRows(worksheet, totalRows, validationColumnProvider);
                    }

                    if (!result)
                    {
                        error = "There are some errors in the uploaded file. Please correct them and upload again.";
                    }
                }
            }

            errorMessage = error;
            errorFile = excelPackage.GetAsByteArray();

            return result;
        }

        private string GetTempFilePath(string tempFileName)
        {
            return Path.Combine(HttpContext.Current.Server.MapPath("~/Content/svgs"), tempFileName);
        }

        private bool ValidateColumns(ExcelWorksheet worksheet, int totalColumns)
        {
            List<string> listColumnHeaders = GetColumnHeaders(worksheet);

            if (totalColumns != listColumnHeaders.Count)
            {
                // Add an appropriate error message
                return false;
            }

            return true;
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
                    columnHeaders.Add(cell.Value.ToString());
                }
            }

            return columnHeaders;
        }

        private bool ValidateHeaderColumns(ExcelWorksheet worksheet, int totalColumns)
        {
            bool result = true;
            List<string> listColumnHeaders = GetColumnHeaders(worksheet);

            for (int i = 1; i <= totalColumns; i++)
            {
                var cell = worksheet.Cells[1, i];

                if (cell.Value != null)
                {
                    if (!listColumnHeaders.Contains(cell.Value.ToString()))
                    {
                        result &= SetError(cell, "Invalid header. Please correct.");
                    }
                }
                else
                {
                    result &= SetError(cell, "Empty header. Remove the column.");
                }
            }

            return result;
        }

        private bool ValidateRows(ExcelWorksheet worksheet, int totalRows, Func<int, string> validationTextProvider)
        {
            bool result = true;

            for (int i = 2; i <= totalRows; i++) // data rows start from 2
            {
                for (int j = 1; j <= worksheet.Dimension.Columns; j++)
                {
                    var cell = worksheet.Cells[i, j];

                    // Use the validationTextProvider function to get the validation text for each column
                    result &= ValidateText(cell, validationTextProvider(j));
                }
            }

            return result;
        }


        private bool SetError(ExcelRange cell, string errorComment)
        {
            var fill = cell.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
            cell.AddComment(errorComment, "");

            return false;
        }

        private bool ValidateText(ExcelRange cell, string columnName)
        {
            bool result = true;
            string error = string.Format("{0} is empty", columnName);

            if (cell.Value != null)
            {
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
    }

}
