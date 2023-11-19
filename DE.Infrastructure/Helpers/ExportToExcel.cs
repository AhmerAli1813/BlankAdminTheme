using OfficeOpenXml;
using System;
using System.Collections.Generic;


public class ExcelExport
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
            }

            // Add data
            for (int row = 0; row < data.Count; row++)
            {
                var rowData = data[row];
                for (int col = 0; col < headers.Length; col++)
                {
                    if (rowData.TryGetValue(headers[col], out object value))
                    {
                        worksheet.Cells[row + 2, col + 1].Value = value;
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
}


