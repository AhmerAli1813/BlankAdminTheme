using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using DPWVessel.Web.Controllers.Api;
using DPWVessel.Web.Core.Model;
using DPWVessel.Web.Core.Session;
using DPWVessel.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DPWVessel.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {
        private readonly IRequestExecutor _requestExecutor;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly dpw_VesselEntities _dbContext;
        private readonly ISessionManager _sessionManager;

        public UsersController(dpw_VesselEntities dbContext, ISessionManager sessionManager, IRequestExecutor requestExecutor)
        {
            _dbContext = dbContext;
            _sessionManager = sessionManager;
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _signInManager = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            _requestExecutor = requestExecutor;

        }
        // GET: Users
        public ActionResult Index() => View();

        public ActionResult ResetPassword() => View();

        public ActionResult Edit() => View();


        public ActionResult Step2() => View();


        [HttpGet]
        public ActionResult ImportUserData() => View();


        [HttpPost]
        public async Task<ActionResult> ImportUserData(ImportExcel importExcel)
        {

            int regcount = 0;
            int newcount = 0;
            try
            {
                if (importExcel.file.ContentLength > 0 && ModelState.IsValid)
                {

                    using (ExcelPackage package = new ExcelPackage(importExcel.file.InputStream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                        int totalRows = workSheet.Dimension.Rows;

                        List<RegisterUserViewModel> StoreList = new List<RegisterUserViewModel>();
                        AccountApiController ob = new AccountApiController(_dbContext, _sessionManager, _requestExecutor);
                        for (int i = 2; i <= totalRows; i++)
                        {
                            try
                            {
                                var row = new RegisterUserViewModel
                                {
                                    Username = workSheet.Cells[i, 1].Value as string,
                                    FullName = workSheet.Cells[i, 2].Value as string,
                                    Email = workSheet.Cells[i, 4].Value as string,
                                    //UserImage = workSheet.Cells[i, 5].Value as string,
                                    Password = workSheet.Cells[i, 6].Value as string,
                                    ConfirmPassword = workSheet.Cells[i, 7].Value as string,
                                    Phone = workSheet.Cells[i, 9].Value as string,
                                    // UserType = importExcel.UserType,
                                };

                                StoreList.Add(row);

                                var res = await ob.ImportUsers(row);
                                if (res.Success) { newcount++; }
                                else { regcount++; }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        ViewBag.Message1 = "File uploaded successfully";
                        ViewBag.Message2 = "New User Registered: " + newcount + " and Already Users Exist: " + regcount;
                        return View();
                    }
                }
            }
            catch
            {
                ViewBag.Message3 = "File upload failed!!";
                return View();
            }
            return View();
        }
    }
}