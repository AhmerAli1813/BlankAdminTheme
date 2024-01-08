using Microsoft.Ajax.Utilities;
using NLog.Targets;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace EasyMe.Web.Controllers
{
    //How passing data in ExporttoExcel here I'm telling you

    //var data = new Dictionary<string, object>
    //                    {
    //                        {"Id", 1004 },
    //                       //Multiple column respective records this is using for single not list item 
    //                    };
    //var data = YourObjecPassinghere.Select(item => new Dictionary<string, object>
    //                    {
    //                        { "Id #", item.id },
    //                     
    //                    }).ToList();
    /// <summary>
    /// Exports data to an Excel file with support for multiple dropdown menus.
    /// </summary>
    //how Passing data  in data 
    /// <param name="data">
    // //var data = new Dictionary<string, object>
    //                    {
    //                        {"Id", 1004 },
    //                       //Multiple column respective records this is using for single not list item 
    //                    };
    //var data = YourObjecPassinghere.Select(item => new Dictionary<string, object>
    //                    {
    //                        { "Id #", item.id },
    //                     
    //                    }).ToList();
    //</param>
    //how passing headers 
    /// How you add Custom styling of every header cloumn here I'm telling you 

    /// <param name="columnStyling">
    ///  var headers = new[] { "Id","so on .........." };
    /// An array of strings representing the column headers repespective Cell.
    /// var columnStyles = new Dictionary<string, Action<ExcelRange>>
    //{
    //here You passing header Column Name and add styling 
    //    { "header1", cell => 
    //        {
    //            cell.Style.Fill.BackgroundColor.SetColor(Color.Red);
    //            cell.Style.Font.Color.SetColor(Color.Yellow);
    //            cell.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Green);
    //            // Add more styling properties as needed
    //        }
    //    },
    //    // Add styles for other headers
    //};

    //var excelBytes = excelExport.ExportToExcel(data, headers, dropdownItems, columnStyles);

    /// </param>
    /// In case You can passing dropdown 
    /// <param name="dropdownItems">
    /// A dictionary containing dropdown menu names as keys and their respective items as values.
    /// remember one thing , DropDown menu 1 also passing in header and data if header does not find DropDown menu 1 then given Excpection
    ///        var dropdownItems = new Dictionary<string, string[]>
    ///         /// <param name="headers">
    ///  var headers = new[] { "Id","so on .........." };
    /// An array of strings representing the column headers.
    /// </param>
    /// In case You can passing dropdown 
    /// <param name="dropdownItems">
    /// A dictionary containing dropdown menu names as keys and their respective items as values.
    /// remember one thing , DropDown menu 1 also passing in header and data if header does not find DropDown menu 1 then given Excpection
    ///        var dropdownItems = new Dictionary<string, string[]>
    //                                        {
    //                                            { "DropDown menu 1", new[] { "item 1", "itme 2",  "other" } },
    //                                            { "DropDown menu 2", new[] { "item 1", "itme 2",  "other" } },
    //                                            { "DropDown menu 3", new[] { "item 1", "itme 2",  "other" } },
    //                                              // so on... 
    //                                        };
    ///// </param>
    /// <returns>Byte array representing the Excel file content.</returns>
    //and how to Download file  this ExcelServices ,
    //ExcelServices given return byte[] array then you download
    //using like this
    //var excelBytes = excelExport.ExportToExcel(data, headers, dropdownItems);
    //string filename = $"Temp_YourFileName_{DateTime.Now.ToString("dd-MM-yyy")}.xlsx";
    //return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
    public sealed class ExcelServices
    {
        private string FunctionErrorMessage { get; set; }
        public string TempFileName { get; set; } = "Sheet1";
        //Overloaded method one start here  with list of Data and headers
        #region Export To Excel Method 
        public byte[] ExportToExcel(List<Dictionary<string, object>> data, string[] headers)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(TempFileName);

                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];

                    // Apply default styling
                    ApplyDefaultHeaderStyle(worksheet.Cells[1, i + 1]);

                    
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
        //Overloaded method one end here
        //Overloaded method two only headers start  here 
        public byte[] ExportToExcel(string[] headers)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(TempFileName);

                // Add headers
                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];

                    // Apply default styling
                    ApplyDefaultHeaderStyle(worksheet.Cells[1, i + 1]);


                }

                // Add data


                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
        //Overloaded method two end here
        //Overloaded method three list of data .header , dropdown start here
        public byte[] ExportToExcel(List<Dictionary<string, object>> data, string[] headers, Dictionary<string, string[]> dropdownItems)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(TempFileName);

       
                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    // Apply default styling
                    ApplyDefaultHeaderStyle(worksheet.Cells[1, i + 1]);
                }


                foreach (var dropdown in dropdownItems)
                {
                    AddDropdownValidation(worksheet, data.Count, dropdown.Key, dropdown.Value);
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
        public byte[] ExportToExcel(Dictionary<string, object> data, string[] headers, Dictionary<string, string[]> dropdownItems)
        {
            return ExportToExcel(new List<Dictionary<string, object>> { data }, headers, dropdownItems);
        }
        //Overloaded method three end here
        //Overloaded method four data.header,dropdown,columnstyling start here
        public byte[] ExportToExcel(List<Dictionary<string, object>> data, string[] headers, Dictionary<string, string[]> dropdownItems, Dictionary<string, Action<ExcelRange>> columnStyling)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(TempFileName);

                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];

                    // Apply default styling
                    ApplyDefaultHeaderStyle(worksheet.Cells[1, i + 1]);

                    // Apply custom styling if specified
                    if (columnStyling.ContainsKey(headers[i]))
                    {
                        columnStyling[headers[i]].Invoke(worksheet.Cells[1, i + 1]);
                    }
                }

                // Add dropdown list data validations for each dropdown
                foreach (var dropdown in dropdownItems)
                {
                    AddDropdownValidation(worksheet, data.Count, dropdown.Key, dropdown.Value);
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
        public byte[] ExportToExcel(Dictionary<string, object> data, string[] headers, Dictionary<string, string[]> dropdownItems, Dictionary<string, Action<ExcelRange>> columnStyling)
        {
            return ExportToExcel(new List<Dictionary<string, object>> { data }, headers, dropdownItems, columnStyling);
        }
        //Overloaded method four end here
        //Overloaded method five start , data ,header , custom styling here
        public byte[] ExportToExcel(List<Dictionary<string, object>> data, string[] headers, Dictionary<string, Action<ExcelRange>> columnStyling)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(TempFileName);

                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];

                    // Apply default styling
                    ApplyDefaultHeaderStyle(worksheet.Cells[1, i + 1]);

                    // Apply custom styling if specified
                    if (columnStyling.ContainsKey(headers[i]))
                    {
                        columnStyling[headers[i]].Invoke(worksheet.Cells[1, i + 1]);
                    }
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
        public byte[] ExportToExcel(Dictionary<string, object> data, string[] headers, Dictionary<string, Action<ExcelRange>> columnStyling)
        {
            return ExportToExcel(new List<Dictionary<string, object>> { data }, headers, columnStyling);
        }
        //Overloaded method five end here 
        //Overloaded method six start  headers and custom styling here
        public byte[] ExportToExcel( string[] headers, Dictionary<string, Action<ExcelRange>> columnStyling)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(TempFileName);

                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];

                    // Apply default styling
                    ApplyDefaultHeaderStyle(worksheet.Cells[1, i + 1]);

                    // Apply custom styling if specified
                    if (columnStyling.ContainsKey(headers[i]))
                    {
                        columnStyling[headers[i]].Invoke(worksheet.Cells[1, i + 1]);
                    }
                }

             

                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
        #endregion
        //Overloaded method six end here
        #region Validation Excel Method
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
        #endregion
        #region Extra Column Validation
        //validation of excel rows column data ....?
        private bool SetError(ExcelRange cell, string errorComment)
        {


            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Red);
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            return false;
        }
        private bool SetError(ExcelRange cell)
        {


            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Red);
           // cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            cell.Style.Font.Color.SetColor(Color.White);
            return false;
        }
        private string SetErrorMessage( string errorMessage)
        {
            // Set the background color to indicate an error
            //cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //cell.Style.Fill.BackgroundColor.SetColor(Color.Red);
            //cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //cell.Style.Font.Color.SetColor(Color.White);

            FunctionErrorMessage += errorMessage;
            return errorMessage; // Return the error message
        } private string SetErrorMessage(ExcelRange cell, string errorMessage)
        {
            // Set the background color to indicate an error
            //cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //cell.Style.Fill.BackgroundColor.SetColor(Color.Red);
            //cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //cell.Style.Font.Color.SetColor(Color.White);

            FunctionErrorMessage += errorMessage;
            return errorMessage; // Return the error message
        }
        public bool ValidateText(ExcelRange cell, string columnName)
        {//this validation is used for in case user send empty row
            bool result = true;
            string error = string.Format("{0} is azxzz empty", columnName);

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
                    result = SetError(cell);
                    FunctionErrorMessage = string.Format("{0} Email address format is invalid ", columnName);
                }
                else if (cell.Value.ToString().Length > 150)
                {
                    result = SetError(cell);
                    FunctionErrorMessage = string.Format("Email: {0} address too long. Max characters 150. ", columnName);
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



            result = ValidateText(cell, columnName);

            if (result)
            {
                if (!double.TryParse(cell.Value.ToString(), out value))
                {
                    FunctionErrorMessage = string.Format("{0} is not Number ", columnName);
                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.Red);
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                   
                    result = false;
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
                    result = SetError(cell);
                    FunctionErrorMessage = string.Format("Date : {0} format is incorrect.", columnName);
                }
            }

            return result;
        }
        private bool IsNumeric(object value)
        {
            return value is sbyte || value is byte || value is short || value is ushort || value is int ||
                   value is uint || value is long || value is ulong || value is float || value is double || value is decimal;
        }
        private string RemoveAsterisk(string value)
        {
            if (value != null && value.Contains("*"))
            {
                value = value.Replace("*", "");
            }
            return value;
        }

        private string ExcelColumnFromNumber(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private void AddDropdownValidation(ExcelWorksheet worksheet, int rowCount, string columnName, string[] dropdownItems)
        {
            var columnNumber = Array.IndexOf(worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(c => c.Text).ToArray(), columnName) + 1;

            if (columnNumber > 0)
            {
                var dropdownRange = $"{ExcelColumnFromNumber(columnNumber)}2:{ExcelColumnFromNumber(columnNumber)}{rowCount + 1}";
                var validation = worksheet.DataValidations.AddListValidation(dropdownRange);

                foreach (var item in dropdownItems)
                {
                    validation.Formula.Values.Add(item);
                }
            }
        }
        private void ApplyDefaultHeaderStyle(ExcelRange cell)
        {
            // Default styling
            Color colFromHex = ColorTranslator.FromHtml("#013447");
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(colFromHex);
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 12;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.Font.Color.SetColor(Color.White);

            // Additional default styling properties
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            // Get the existing text of the cell
            var text = cell.Text;
            if (text.Contains("*"))
            {
                // Find the index of the asterisk (*) in the text
                int asteriskIndex = text.IndexOf('*');

                // Clear existing text formatting
                cell.RichText.Clear();

                // Add text before asterisk with white color
                cell.RichText.Add(text.Substring(0, asteriskIndex)).Color = Color.White;

                // Add asterisk with red color
                cell.RichText.Add("*").Color = Color.Red;

                // Add text after asterisk with white color
                cell.RichText.Add(text.Substring(asteriskIndex + 1)).Color = Color.White;
            }

        }
        #endregion

        #region SaveInvalidRecordsToExcel Method Start here 
        public byte[] SaveInvalidRecordsToExcel(ExcelWorksheet worksheet, List<int> invalidRows)
        {
            using (var package = new ExcelPackage())
            {
                var invalidSheet = package.Workbook.Worksheets.Add(TempFileName);
                Color colFromHex = ColorTranslator.FromHtml("#013447");

                // Copy headers from the original sheet to the new sheet, and add 'Remark' header
                for (int col = 1; col <= worksheet.Dimension.End.Column + 1; col++)
                {
                    if (col <= worksheet.Dimension.End.Column)
                    {
                        invalidSheet.Cells[1, col].Value = worksheet.Cells[1, col].Value;
                    }
                    else
                    {
                        invalidSheet.Cells[1, col].Value = "Remark";
                    }

                    invalidSheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    invalidSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    invalidSheet.Cells[1, col].Style.Font.Bold = true;
                    invalidSheet.Cells[1, col].Style.Font.Size = 12;
                    invalidSheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    invalidSheet.Cells[1, col].Style.Font.Color.SetColor(Color.White);
                }

                // Mark invalid rows with red color and add comments
                foreach (int row in invalidRows)
                {
                    bool rowIsValid = true;

                    for (int col = 1; col <= worksheet.Dimension.End.Column + 1; col++)
                    {
                        var sourceCell = col <= worksheet.Dimension.End.Column ? worksheet.Cells[row, col] : null;
                        var invalidCell = invalidSheet.Cells[row, col];

                        if (col <= worksheet.Dimension.End.Column)
                        {
                            invalidCell.Value = sourceCell.Value;

                            var fill = invalidCell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;

                            if (string.IsNullOrWhiteSpace(sourceCell.Text))
                            {
                                fill.BackgroundColor.SetColor(Color.Red);
                                

                                // Get the current value in the 'Remark' column
                                var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;

                                // Set the 'Remark' column value
                                invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Value = string.IsNullOrEmpty(currentRemark)
                                    ? $"{worksheet.Cells[1, col].Text} is empty."
                                    : $"{currentRemark}, {worksheet.Cells[1, col].Text} is empty.";

                                rowIsValid = false;
                            }
                            else
                            {
                                // If the cell has a value, do not fill the background color
                                fill.BackgroundColor.SetColor(Color.White);
                                
                            }
                        }
                        else
                        {
                            // Only set the 'Remark' column value if there is an issue in the row
                            if (!rowIsValid)
                            {
                                // Get the current value in the 'Remark' column
                                var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;

                                // Set the 'Remark' column value
                                invalidSheet.Cells[row, col].Value = string.IsNullOrEmpty(currentRemark)
                                    ? "No issues"
                                    : $"{currentRemark}";
                            }
                        }
                    }

                    if (rowIsValid)
                    {
                        invalidSheet.Cells[invalidSheet.Dimension.End.Row + 1, 1, invalidSheet.Dimension.End.Row + 1, invalidSheet.Dimension.End.Column].Value
                            = invalidSheet.Cells[row, 1, row, invalidSheet.Dimension.End.Column].Value;
                    }
                }




                // Delete empty rows
                for (int row = invalidSheet.Dimension.Start.Row; row <= invalidSheet.Dimension.End.Row; row++)
                {
                    bool rowIsEmpty = true;
                    for (int col = invalidSheet.Dimension.Start.Column; col <= invalidSheet.Dimension.End.Column; col++)
                    {
                        var cell = invalidSheet.Cells[row, col];
                        if (!string.IsNullOrWhiteSpace(cell.Text))
                        {
                            rowIsEmpty = false;
                            break;
                        }
                    }

                    if (rowIsEmpty)
                    {
                        invalidSheet.DeleteRow(row);
                        row--; // Adjust the row index after deletion
                    }
                }

                invalidSheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }
        public byte[] SaveInvalidRecordsToExcel(ExcelWorksheet worksheet, List<int> invalidRows, Dictionary<int, Func<ExcelRange, string, bool>> validationMethods)
        {
            string errorMsg = string.Empty ;
            using (var package = new ExcelPackage())
            {
                var invalidSheet = package.Workbook.Worksheets.Add(TempFileName);
                Color colFromHex = ColorTranslator.FromHtml("#013447");

                for (int col = 1; col <= worksheet.Dimension.End.Column + 1; col++)
                {
                    if (col <= worksheet.Dimension.End.Column)
                    {
                        invalidSheet.Cells[1, col].Value = worksheet.Cells[1, col].Value;
                    }
                    else
                    {
                        invalidSheet.Cells[1, col].Value = "Remark";
                    }

                    invalidSheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    invalidSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    invalidSheet.Cells[1, col].Style.Font.Bold = true;
                    invalidSheet.Cells[1, col].Style.Font.Size = 12;
                    invalidSheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    invalidSheet.Cells[1, col].Style.Font.Color.SetColor(Color.White);
                }

                // Mark invalid rows with red color and Reamrk Value
                foreach (int row in invalidRows)
                {
                    bool rowIsValid = true;

                    for (int col = 1; col <= worksheet.Dimension.End.Column + 1; col++)
                    {
                        var sourceCell = col <= worksheet.Dimension.End.Column ? worksheet.Cells[row, col] : null;
                        var invalidCell = invalidSheet.Cells[row, col];

                        if (col <= worksheet.Dimension.End.Column)
                        {
                            invalidCell.Value = sourceCell.Value;

                            var fill = invalidCell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;


                            if (string.IsNullOrWhiteSpace(sourceCell.Text))
                            {
                                fill.BackgroundColor.SetColor(Color.Red);
                                

                                FunctionErrorMessage  += $"{RemoveAsterisk(worksheet.Cells[1, col].Text)} is empty";
                                // Get the current value in the 'Remark' column
                                var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;

                                // Set the 'Remark' column value
                                invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Value = string.IsNullOrEmpty(currentRemark)
                                    ? FunctionErrorMessage
                                    : $"{FunctionErrorMessage}";

                                rowIsValid = false;
                            }
                            else
                            {
                                // If the cell has a value, do not fill the background color
                                fill.BackgroundColor.SetColor(Color.White);
                                
                            }

                            // Check if a validation method is provided for this column
                            // Check if a validation method is provided for this column
                            if (validationMethods.TryGetValue(col, out var validationMethod))
                            {
                                // Perform validation using the provided method
                                if (!validationMethod(sourceCell, invalidCell.Text))
                                {
                                    

                                     SetError(invalidCell);
                                    // Get the current value in the 'Remark' column
                                    var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;

                                    // Set the 'Remark' column value for validation failure
                                    invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Value = string.IsNullOrEmpty(currentRemark)
                                        ? FunctionErrorMessage
                                        : $"{FunctionErrorMessage}";

                                    rowIsValid = false;
                                }
                                else
                                {
                                    // If the cell has a value, do not fill the background color
                                    //fill.BackgroundColor.SetColor(Color.Red);
                                    //
                                }
                            }

                        }
                        else
                        {
                            // Only set the 'Remark' column value if there is an issue in the row
                            if (!rowIsValid)
                            {
                                var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;
                                var error = SetErrorMessage(invalidCell,null);

                                // Set the 'Remark' column value
                                invalidSheet.Cells[row, col].Value = string.IsNullOrEmpty(currentRemark)
                                    ? error
                                    : $"{currentRemark} {error}";
                            }
                        }
                    }
                    FunctionErrorMessage = string.Empty;
                    if (rowIsValid)
                    {
                        invalidSheet.Cells[invalidSheet.Dimension.End.Row + 1, 1, invalidSheet.Dimension.End.Row + 1, invalidSheet.Dimension.End.Column].Value
                            = invalidSheet.Cells[row, 1, row, invalidSheet.Dimension.End.Column].Value;
                    }
                }




                // Delete empty rows
                for (int row = invalidSheet.Dimension.Start.Row; row <= invalidSheet.Dimension.End.Row; row++)
                {
                    bool rowIsEmpty = true;
                    for (int col = invalidSheet.Dimension.Start.Column; col <= invalidSheet.Dimension.End.Column; col++)
                    {
                        var cell = invalidSheet.Cells[row, col];
                        if (!string.IsNullOrWhiteSpace(cell.Text))
                        {
                            rowIsEmpty = false;
                            break;
                        }
                    }

                    if (rowIsEmpty)
                    {
                        invalidSheet.DeleteRow(row);
                        row--; // Adjust the row index after deletion
                    }
                }

                invalidSheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        } 
        public byte[] SaveInvalidRecordsToExcel(ExcelWorksheet worksheet, List<int> invalidRows, Dictionary<int, Func<ExcelRange, string, bool>> validationMethods, Dictionary<string, string[]> dropdownItems)
        {
            string errorMsg = string.Empty ;
            using (var package = new ExcelPackage())
            {
                var invalidSheet = package.Workbook.Worksheets.Add(TempFileName);
                Color colFromHex = ColorTranslator.FromHtml("#013447");

                for (int col = 1; col <= worksheet.Dimension.End.Column + 1; col++)
                {
                    if (col <= worksheet.Dimension.End.Column)
                    {
                        invalidSheet.Cells[1, col].Value = worksheet.Cells[1, col].Value;
                    }
                    else
                    {
                        invalidSheet.Cells[1, col].Value = "Remark";
                    }

                    invalidSheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    invalidSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    invalidSheet.Cells[1, col].Style.Font.Bold = true;
                    invalidSheet.Cells[1, col].Style.Font.Size = 12;
                    invalidSheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    invalidSheet.Cells[1, col].Style.Font.Color.SetColor(Color.White);
                }

                // Mark invalid rows with red color and Reamrk Value
                foreach (int row in invalidRows)
                {
                    bool rowIsValid = true;

                    for (int col = 1; col <= worksheet.Dimension.End.Column + 1; col++)
                    {
                        var sourceCell = col <= worksheet.Dimension.End.Column ? worksheet.Cells[row, col] : null;
                        var invalidCell = invalidSheet.Cells[row, col];

                        if (col <= worksheet.Dimension.End.Column)
                        {
                            invalidCell.Value = sourceCell.Value;

                            var fill = invalidCell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;


                            if (string.IsNullOrWhiteSpace(sourceCell.Text))
                            {
                                fill.BackgroundColor.SetColor(Color.Red);
                                

                                FunctionErrorMessage  += $"{RemoveAsterisk(worksheet.Cells[1, col].Text)} is empty, ";
                                // Get the current value in the 'Remark' column
                                var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;

                                // Set the 'Remark' column value
                                invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Value = string.IsNullOrEmpty(currentRemark)
                                    ? FunctionErrorMessage
                                    : $"{FunctionErrorMessage}";

                                rowIsValid = false;
                            }
                            else
                            {
                                // If the cell has a value, do not fill the background color
                                fill.BackgroundColor.SetColor(Color.White);
                                
                            }

                            // Check if a validation method is provided for this column
                            // Check if a validation method is provided for this column
                            if (validationMethods.TryGetValue(col, out var validationMethod))
                            {
                                // Perform validation using the provided method
                                if (!validationMethod(sourceCell, invalidCell.Text))
                                {
                                    

                                     SetError(invalidCell);
                                    // Get the current value in the 'Remark' column
                                    var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;

                                    // Set the 'Remark' column value for validation failure
                                    invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Value = string.IsNullOrEmpty(currentRemark)
                                        ? FunctionErrorMessage
                                        : $"{FunctionErrorMessage}";

                                    rowIsValid = false;
                                }
                                else
                                {
                                    // If the cell has a value, do not fill the background color
                                    //fill.BackgroundColor.SetColor(Color.Red);
                                    //
                                }
                            }

                        }
                        else
                        {
                            // Only set the 'Remark' column value if there is an issue in the row
                            if (!rowIsValid)
                            {
                                var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;
                                var error = SetErrorMessage(invalidCell,null);

                                // Set the 'Remark' column value
                                invalidSheet.Cells[row, col].Value = string.IsNullOrEmpty(currentRemark)
                                    ? error
                                    : $"{currentRemark},-shhs--";
                            }
                        }
                    }
                    FunctionErrorMessage = string.Empty;
                    if (rowIsValid)
                    {
                        invalidSheet.Cells[invalidSheet.Dimension.End.Row + 1, 1, invalidSheet.Dimension.End.Row + 1, invalidSheet.Dimension.End.Column].Value
                            = invalidSheet.Cells[row, 1, row, invalidSheet.Dimension.End.Column].Value;
                    }
                }
                #region dropdown code is not working
                //foreach (var dropdown in dropdownItems)
                //{
                //    // Assuming dropdown.Key represents the column name
                //    var columnName = dropdown.Key;
                //    var dropdownValues = dropdown.Value;

                //    // Assuming invalidRows.Count represents the number of invalid rows
                //    var rowCount = invalidRows.Count;

                //    // Find the column index based on the column name
                //    var colIndex = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
                //        .First(c => c.Text == columnName).Start.Column;

                //    // Clear all existing data validations in the specified column
                //    worksheet.DataValidations.RemoveAll(v => v.Address.Start.Column == colIndex);

                //    // Set the data validation for each cell in the specified column
                //    for (int row = 2; row <= rowCount + 1; row++)
                //    {
                //        var validationCell = worksheet.Cells[row, colIndex];

                //        // Set data validation formula for the cell
                //        validationCell.Formula = $"{columnName}{row}";

                //        // Set dropdown values for the validation formula
                //        var validation = validationCell.DataValidation.AddListDataValidation();
                //        foreach (var item in dropdownValues)
                //        {
                //            validation.Formula.Values.Add(item);
                //        }

                //        // Set error message for invalid entries
                //        validation.ShowErrorMessage = true;
                //        validation.ErrorTitle = "Invalid Entry";
                //        validation.Error = "Please select a value from the dropdown list.";
                //    }
                //}


                #endregion

                // Delete empty rows
                for (int row = invalidSheet.Dimension.Start.Row; row <= invalidSheet.Dimension.End.Row; row++)
                {
                    bool rowIsEmpty = true;
                    for (int col = invalidSheet.Dimension.Start.Column; col <= invalidSheet.Dimension.End.Column; col++)
                    {
                        var cell = invalidSheet.Cells[row, col];
                        if (!string.IsNullOrWhiteSpace(cell.Text))
                        {
                            rowIsEmpty = false;
                            break;
                        }
                    }

                    if (rowIsEmpty)
                    {
                        invalidSheet.DeleteRow(row);
                        row--; // Adjust the row index after deletion
                    }
                }

                invalidSheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }  
        public byte[] SaveInvalidRecordsToExcel(ExcelWorksheet worksheet, List<int> invalidRows, Dictionary<int, Func<ExcelRange, string, bool>> validationMethods, Dictionary<string, string[]> dropdownItems, Dictionary<int, string> excludedRows)
        {
            string errorMsg = string.Empty ;
            using (var package = new ExcelPackage())
            {
                var invalidSheet = package.Workbook.Worksheets.Add(TempFileName);
                Color colFromHex = ColorTranslator.FromHtml("#013447");

                for (int col = 1; col <= worksheet.Dimension.End.Column + 1; col++)
                {
                    if (col <= worksheet.Dimension.End.Column)
                    {
                        invalidSheet.Cells[1, col].Value = worksheet.Cells[1, col].Value;
                    }
                    else
                    {
                        invalidSheet.Cells[1, col].Value = "Remark";
                    }

                    invalidSheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    invalidSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    invalidSheet.Cells[1, col].Style.Font.Bold = true;
                    invalidSheet.Cells[1, col].Style.Font.Size = 12;
                    invalidSheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    invalidSheet.Cells[1, col].Style.Font.Color.SetColor(Color.White);
                }
                int remarkColumnIndex = worksheet.Dimension.End.Column + 1;
                // Mark invalid rows with red color and Reamrk Value
                foreach (int row in invalidRows)
                {
                    bool rowIsValid = true;

                    for (int col = 1; col <= worksheet.Dimension.End.Column + 1; col++)
                    {
                        var sourceCell = col <= worksheet.Dimension.End.Column ? worksheet.Cells[row, col] : null;
                        var invalidCell = invalidSheet.Cells[row, col];

                        if (col <= worksheet.Dimension.End.Column)
                        {
                            invalidCell.Value = sourceCell.Value;

                            var fill = invalidCell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;

                            if (string.IsNullOrWhiteSpace(sourceCell.Text))
                            {
                                fill.BackgroundColor.SetColor(Color.Red);
                                if (FunctionErrorMessage == string.Empty)
                                {
                                    FunctionErrorMessage = $"{RemoveAsterisk(worksheet.Cells[1, col].Text)} is empty ";
                                }
                                else{
                                    FunctionErrorMessage += $", {RemoveAsterisk(worksheet.Cells[1, col].Text)} is empty";
                                }
                               

                                var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;
                                invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Value = string.IsNullOrEmpty(currentRemark)
                                    ? FunctionErrorMessage
                                    : $",{FunctionErrorMessage}";

                                rowIsValid = false;
                            }
                            else
                            {
                                fill.BackgroundColor.SetColor(Color.White);
                            }

                            if (validationMethods.TryGetValue(col, out var validationMethod))
                            {
                                if (!validationMethod(sourceCell, invalidCell.Text))
                                {
                                    SetError(invalidCell);

                                    var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;
                                    invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Value = string.IsNullOrEmpty(currentRemark)
                                        ? FunctionErrorMessage
                                        : $"{FunctionErrorMessage}";

                                    rowIsValid = false;
                                }
                            }
                        }
                        else
                        {
                            if (!rowIsValid)
                            {
                                //var currentRemark = invalidSheet.Cells[row, worksheet.Dimension.End.Column + 1].Text;
                                //var error = SetErrorMessage(invalidCell, null);

                                //invalidSheet.Cells[row, col].Value = string.IsNullOrEmpty(currentRemark)
                                //    ? error
                                //    : $"{currentRemark} , ---";
                            }
                        }
                    }

                    // Handle excluded rows
                    if (excludedRows.ContainsKey(row) && !string.IsNullOrEmpty(excludedRows[row]))
                    {
                        invalidSheet.Cells[row, remarkColumnIndex].Value =  excludedRows[row];
                        rowIsValid = false;
                    }

                    FunctionErrorMessage = string.Empty;

                    // Copy the row only if it is valid
                    if (rowIsValid)
                    {
                        invalidSheet.Cells[invalidSheet.Dimension.End.Row + 1, 1, invalidSheet.Dimension.End.Row + 1, invalidSheet.Dimension.End.Column].Value
                            = invalidSheet.Cells[row, 1, row, invalidSheet.Dimension.End.Column].Value;
                    }
                }

                #region dropdown code is not working
                //foreach (var dropdown in dropdownItems)
                //{
                //    // Assuming dropdown.Key represents the column name
                //    var columnName = dropdown.Key;
                //    var dropdownValues = dropdown.Value;

                //    // Assuming invalidRows.Count represents the number of invalid rows
                //    var rowCount = invalidRows.Count;

                //    // Find the column index based on the column name
                //    var colIndex = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
                //        .First(c => c.Text == columnName).Start.Column;

                //    // Clear all existing data validations in the specified column
                //    worksheet.DataValidations.RemoveAll(v => v.Address.Start.Column == colIndex);

                //    // Set the data validation for each cell in the specified column
                //    for (int row = 2; row <= rowCount + 1; row++)
                //    {
                //        var validationCell = worksheet.Cells[row, colIndex];

                //        // Set data validation formula for the cell
                //        validationCell.Formula = $"{columnName}{row}";

                //        // Set dropdown values for the validation formula
                //        var validation = validationCell.DataValidation.AddListDataValidation();
                //        foreach (var item in dropdownValues)
                //        {
                //            validation.Formula.Values.Add(item);
                //        }

                //        // Set error message for invalid entries
                //        validation.ShowErrorMessage = true;
                //        validation.ErrorTitle = "Invalid Entry";
                //        validation.Error = "Please select a value from the dropdown list.";
                //    }
                //}


                #endregion

                // Delete empty rows
                for (int row = invalidSheet.Dimension.Start.Row; row <= invalidSheet.Dimension.End.Row; row++)
                {
                    bool rowIsEmpty = true;
                    for (int col = invalidSheet.Dimension.Start.Column; col <= invalidSheet.Dimension.End.Column; col++)
                    {
                        var cell = invalidSheet.Cells[row, col];
                        if (!string.IsNullOrWhiteSpace(cell.Text))
                        {
                            rowIsEmpty = false;
                            break;
                        }
                    }

                    if (rowIsEmpty)
                    {
                        invalidSheet.DeleteRow(row);
                        row--; // Adjust the row index after deletion
                    }
                }

                invalidSheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }
        #endregion
    }

}
