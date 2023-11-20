using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using DPWVessel.Model.Features.Equipments;
using DPWVessel.Model.Features.Shared;
using DPWVessel.Web.Core.Session;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Add()
        {

            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

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

        //[HttpGet]
        //public ActionResult ExportToExcel(GetAllEquipmentsRequsted req)

        //{
        //    var resp = _requestExecutor.Execute(req);
        //    using (var excel = new ExcelPackage())
        //    {
        //        var workSheet = excel.Workbook.Worksheets.Add("Worksheet Name");
        //        workSheet.Cells[1, 1].LoadFromCollection(resp.EquipmentsLists, PrintHeaders: true, TableStyle: OfficeOpenXml.Table.TableStyles.Medium6);
        //        workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
        //        return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
        //    }

        //}
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
        public ActionResult Upload(HttpPostedFileBase file)
        {


            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    using (var stream = file.InputStream)
                    {
                        using (ExcelPackage package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                            int totalRows = worksheet.Dimension.Rows;
                            AddEquipmentsRequsted req = new AddEquipmentsRequsted();
                            UpdateEquipmentsInformationRequest ureq = new UpdateEquipmentsInformationRequest();
                            for (int i = 2; i <= totalRows; i++)
                            {
                                int Id = Convert.ToInt32(worksheet.Cells[i, 1].Value);
                                int typeId = Convert.ToInt32(worksheet.Cells[i, 2].Value);
                                object NameValue = worksheet.Cells[i, 3].Value;
                                string Name;
                                if (NameValue==null|| string.IsNullOrWhiteSpace(NameValue.ToString()))
                                {
                                    Name = null;
                                }
                                else
                                {
                                    Name = NameValue.ToString();

                                }
                                
                                string UserName = _sessionManager.CurrentUser.UserName;

                                var t =  _dbContext.Equipments.Find(Id);

                                if (t != null)
                                {
                                    if( typeId > 0 && Name !=null)
                                    {

                                        ureq.data.id = Id;
                                        ureq.data.equipmentTypeId = typeId;
                                        ureq.data.name = Name;
                                        ureq.data.updatedBy = UserName;
                                        _requestExecutor.Execute(ureq);
                                    }
                                }
                                else
                                {
                                    if (typeId > 0 && Name != null)
                                    {

                                        req.equipmentsTypeId = typeId;
                                        req.name = Name;
                                        req.createdBy = UserName;
                                        req.updatedBy = UserName;

                                        _requestExecutor.Execute(req);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { msg = ex.Message, Istrue = false });
                }
            }
            else
            {
                // Handle the case when no file is selected
                return Json(new { msg = "Error file is not selected", Isture = false });
            }

            // Assuming the operation was successful
            return Json(new { msg = "Records processed successfully.", Istrue = true });
        }


        [HttpGet]
        public ActionResult ImportUserData()
        {

            return View();
        }

    }
}