
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DPWVessel.Model.Features.Shared
{
    public class ExcelExportServices
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
        private bool IsNumeric(object value)
        {
            return value is int || value is decimal || value is float || value is double;
        }
    }



}