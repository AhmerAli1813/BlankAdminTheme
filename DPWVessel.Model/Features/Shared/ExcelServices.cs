using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace DPWVessel.Model.Features.Shared
{

    public class ExcelServices
    {
        public byte[] ExportToExcel(List<Dictionary<string, object>> data, string[] headers)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    Color colFromHex = ColorTranslator.FromHtml("#013447");
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Font.Size = 12;
                    worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(Color.White);
                }

                // Add data
                for (int row = 0; row < data.Count; row++)
                {
                    var rowData = data[row];
                    for (int col = 0; col < headers.Length; col++)
                    {
                        if (rowData.TryGetValue(headers[col], out object value))
                        {
                            if (value is DateTime dateValue)
                            {
                                // Format date values
                                worksheet.Cells[row + 2, col + 1].Value = dateValue.ToString("dd-MM-yyyy");
                                worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "dd-MM-yyyy";
                            }
                            else
                            {
                                worksheet.Cells[row + 2, col + 1].Value = value;
                            }

                            // Set alignment based on data type
                            if (IsNumeric(value))
                            {
                                worksheet.Cells[row + 2, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                            else
                            {
                                worksheet.Cells[row + 2, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        }
                        else
                        {
                            // Handle missing data or provide a default value
                            worksheet.Cells[row + 2, col + 1].Value = DBNull.Value;
                        }
                    }
                }

                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        public byte[] ExportToExcel(Dictionary<string, object> data, string[] headers)
        {
            return ExportToExcel(new List<Dictionary<string, object>> { data }, headers);
        }
        public byte[] ExportToExcel(string[] headers)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    Color colFromHex = ColorTranslator.FromHtml("#013447");
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Font.Size = 12;
                    worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(Color.White);
                }

                // Add data


                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
        public bool ValidateExcel(string tempFileName, out string errorMessage, out byte[] ErrorFile, List<string> expectedHeaders, Dictionary<int, Func<ExcelRange, string, bool>> validationMethods)
        {
            bool result = true;
            string error = string.Empty;
            //  string filePath = GetTempFilePath(tempFileName);
            FileInfo fileInfo = new FileInfo(tempFileName);
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
                else if (CheckExcelHeaderFormat(worksheet, expectedHeaders))
                {
                    result = false;
                    error = "Spread sheet column header indexing mismatch.";
                }
                // Check if there are more than or equal to 2 rows
                else if (totalRows < 2)
                {
                    result = false;
                    error = "Spreadsheet does not contain enough data rows.";
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
                        result &= ValidateRows(worksheet, totalRows, totalCols, validationMethods);
                    }

                    if (!result)
                    {
                        error = "There are some errors in the uploaded file. Please correct them and upload again.";
                    }
                }

            }

            errorMessage = error;
            worksheet.Cells.AutoFitColumns();
            ErrorFile = excelPackage.GetAsByteArray();
            // excelPackage.Save();

            return result;
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
        private bool SetError(ExcelRange cell, string errorComment)
        {
            if (cell.Comment == null)
            {
                // Set the error comment
                cell.AddComment(errorComment, "");
            }
            else
            {
                // Update the existing comment
                cell.Comment.Text = errorComment;
            }

            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Red);
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            return false;
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
        public bool CheckExcelHeaderFormat(ExcelWorksheet worksheet, List<string> expectedHeaders)
        {
            bool result = false;
            List<string> actualHeaders = GetColumnHeaders(worksheet);

            for (int i = 0; i < expectedHeaders.Count; i++)
            {
                if (i >= actualHeaders.Count || !string.Equals(expectedHeaders[i], actualHeaders[i], StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        private bool ValidateRows(ExcelWorksheet worksheet, int totalRows, int totalCols, Dictionary<int, Func<ExcelRange, string, bool>> validationMethods)
        {
            bool result = true;

            for (int i = 2; i <= totalRows; i++) // Data rows start from 2
            {
                for (int j = 1; j <= totalCols; j++)
                {
                    var cell = worksheet.Cells[i, j];

                    // Check if the validation method exists for the current column
                    if (validationMethods.TryGetValue(j, out var validationMethod))
                    {
                        // Validate the cell using the appropriate validation method
                        result &= validationMethod(cell, $"Validation for column {j}");
                    }
                    else
                    {
                        // Handle the case where no validation method is defined for the column
                        Console.WriteLine($"No validation method defined for column {j}");
                    }
                }
            }

            return result;
        }

        //validation of excel rows column data ....?
        public bool ValidateText(ExcelRange cell, string columnName)
        {//this validation is used for in case user send empty row
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
        public bool ValidateEmailAddress(ExcelRange cell, string columnName)
        { // this validation for email address is not empty and correct format like: Email@gmail.com
            bool result = true;
            result = ValidateText(cell, columnName); //validate if empty or not

            if (result)
            {
                if (!ValidateEmail(cell.Value.ToString())) //ValidateEmail => true, if email format is correct
                {
                    result = SetError(cell, "Email address format is invalid.");
                }
                else if (cell.Value.ToString().Length > 150)
                {
                    result = SetError(cell, "Email address too long. Max characters 150.");
                }
            }

            return result;
        }
        static bool ValidateEmail(string email)
        {
            // A simple regular expression for common email patterns
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
        public bool ValidateNumber(ExcelRange cell, string columnName)
        {
            bool result = true;
            double value = 0.0;
            string error = string.Format("{0} format is incorrect.", columnName);
            result = ValidateText(cell, columnName);

            if (result)
            {
                if (!double.TryParse(cell.Value.ToString(), out value))
                {
                    result = SetError(cell, error);
                }
            }

            return result;
        }
        public bool ValidateDate(ExcelRange cell, string columnName)
        {
            bool result = true;
            DateTime date = DateTime.MinValue;
            string error = string.Format("{0} format is incorrect.", columnName);
            result = ValidateText(cell, columnName);

            if (result)
            {
                if (!DateTime.TryParse(cell.Value.ToString(), out date))
                {
                    result = SetError(cell, error);
                }
            }

            return result;
        }
        public byte[] SaveInvalidRecordsToExcel(ExcelWorksheet worksheet, List<int> invalidRows)
        {
            using (var package = new ExcelPackage())
            {
                var invalidSheet = package.Workbook.Worksheets.Add("InvalidRecords");
                Color colFromHex = ColorTranslator.FromHtml("#013447");
                // Copy headers from the original sheet to the new sheet
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    invalidSheet.Cells[1, col].Value = worksheet.Cells[1, col].Value;
                    invalidSheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    invalidSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    invalidSheet.Cells[1, col].Style.Font.Bold = true;
                    invalidSheet.Cells[1, col].Style.Font.Size = 12;
                    invalidSheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    invalidSheet.Cells[1, col].Style.Font.Color.SetColor(Color.White);
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
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.Red);
                        invalidCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        invalidCell.AddComment("Record not inserted", "");
                    }
                }
                invalidSheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }
        private bool IsNumeric(object value)
        {
            return value is int || value is decimal || value is float || value is double;
        }

    }

}
