using DPWVessel.Model.EntityModel;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DPWVessel.Model.Features.Shared
{

    public class ExcelImportService<T> where T : class
    {
        private readonly dpw_VesselEntities _dbContext;

        public ExcelImportService(dpw_VesselEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ImportFromExcelAsync(Stream excelFileStream)
        {
            using (var package = new ExcelPackage(excelFileStream))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                {
                    throw new InvalidOperationException("Excel file is empty or does not contain any worksheets.");
                }

                var properties = typeof(T).GetProperties().Where(p => p.CanWrite).ToList();

                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    var entity = Activator.CreateInstance<T>();

                    foreach (var property in properties)
                    {
                        var columnIndex = GetColumnIndex(property);
                        var cellValue = worksheet.Cells[row, columnIndex].Value;

                        if (cellValue != null)
                        {
                            var convertedValue = Convert.ChangeType(cellValue, property.PropertyType);
                            property.SetValue(entity, convertedValue);
                        }
                    }

                    _dbContext.Set<T>().Add(entity);
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        private int GetColumnIndex(PropertyInfo property)
        {
            // You may need a mapping between property names and Excel column headers
            // For simplicity, this example assumes the property name matches the header
            // Adjust this logic based on your actual mapping requirements.

            var columnName = property.Name;
            var worksheetColumn = 1; // Default to the first column

            // You may add a mapping logic here based on your requirements

            return worksheetColumn;
        }
    }

}
