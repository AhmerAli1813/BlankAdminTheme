using DE.Infrastructure.Concept;
using DPWVessel.Model.EntityModel;
using DPWVessel.Model.EntityModel.Enums;
using DPWVessel.Model.Features.Account;
//using DPWVessel.Model.Features.Account;
using DPWVessel.Model.Features.Shared;
using DPWVessel.Model.Resources;
using DPWVessel.Web.Core.Session;
using DPWVessel.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

//using FluentValidation.Resources;

namespace DPWVessel.Web.Controllers.Api
{

    public class LoginResponse : DE.Infrastructure.Concept.Response
    {
        public string UserToken { get; set; }
        public UserInformation User { get; set; }
    }
    public class AccountApiController : BaseApiController
    {
        private new readonly IRequestExecutor _requestExecutor;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly dpw_VesselEntities _dbContext;
        private readonly ISessionManager _sessionManager;
        public AccountApiController(dpw_VesselEntities dbContext, ISessionManager sessionManager, IRequestExecutor requestExecutor)
        {
            _dbContext = dbContext;
            _sessionManager = sessionManager;
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _signInManager = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            _requestExecutor = requestExecutor;
        }
        public class RegisterResponse : DE.Infrastructure.Concept.Response
        {
            public string UserToken { get; set; }
            public UserInformation User { get; set; }
        }

        [HttpGet]
        public object GetUserRole()
        {
            var request = new GetEnumDropdownRequest { DropdownEnum = typeof(UserRole) };
            var test = new GetEnumDropdownRequestHandler();
            GetEnumDropdownResponse dropdown = test.Execute(request);

            return dropdown;
            //}

            //return _requestExecutor.Execute(request);
        }
        [HttpGet]
        public object CheckAccess()
        {
            //string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //string controllerName = ControllerContext.RouteData.Values["controller"].ToString();

            var req = new CheckAccessRequest();
            req.UserApplication = _sessionManager.CurrentUser.UsersApplication.Count();
            req.UserId = _sessionManager.CurrentUser.UserId;
            req.UserType = _sessionManager.CurrentUser.UserType.ToArray();
            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpGet]
        public object GetRolesList()
        {
            var req = new GetRolesListRequest();
            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpPost]
        public async Task<object> ChangePassword(ChangePasswordViewModel model)
        {
            ChangePasswordResponse response = new ChangePasswordResponse();
            if (ModelState.IsValid)
            {
                var Userobject = _sessionManager.CurrentUser;
                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var resp = await userManager.ChangePasswordAsync(Userobject.Id, model.OldPassword, model.NewPassword);
                if (resp.Succeeded)
                {
                    return resp;
                }

                else
                {
                    response.AssignValidationErrors(resp.Errors);
                    return response;
                }

            }

            response.AssignValidationErrors(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return response;
        }
        public bool ValidateRequiredFields(RegisterUserViewModel model, out string errorMesage)
        {
            foreach (var item in model.UserType)
            {
                var usertype = (UserRole)item;
                //errorMesage = "Success";
                //return true;
            }
            errorMesage = "Success";
            return true;
        }


        [HttpGet]
        public object GetUserRoles()
        {
            var request = new GetEnumDropdownRequest { DropdownEnum = typeof(UserRole) };
            var test = new GetEnumDropdownRequestHandler();
            GetEnumDropdownResponse dropdown = test.Execute(request);
            return dropdown;

        }

        [HttpGet]
        public object GetUserProfileDetails()
        {
            var req = new UserProfileDetailsRequest();
            req.UserId = _sessionManager.CurrentUser.Id;
            var resp = _requestExecutor.Execute(req);
            return resp;
        }




        [HttpPost]
        public object UpdateProfile(EditProfileRequest req)
        {
            req.UserId = _sessionManager.CurrentUser.Id;
            var resp = _requestExecutor.Execute(req);
            return resp;
        }

        [HttpPost]
        public async Task<Response> Register(RegisterUserViewModel model)
        {

            string errorMessage = "!";
            if (ValidateRequiredFields(model, out errorMessage) != true)
            {
                return RegisterResponse.GetInvalidResponse(errorMessage);

            }
            else
            {

                if (ModelState.IsValid)
                {

                    var user = new ApplicationUser
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        PhoneNumber = model.Phone,
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        ExceptionDispatchInfo capturedException = null;
                        try
                        {

                            if (model.UserType.Count() > 0)
                            {
                                foreach (var item in model.UserType)
                                {
                                    if ((item == 1) || (model.UsersApplication.Contains("1") && item == 2) || (model.UsersApplication.Contains("2") && item == 2) || (model.UsersApplication.Contains("2") && item == 3))
                                    {
                                        var usertype = (UserRole)item;
                                        await _userManager.AddToRoleAsync(user.Id, usertype.ToString());
                                    }
                                    if ((model.UsersApplication.Contains("1") && item == 4) || (model.UsersApplication.Contains("1") && item == 5) || (model.UsersApplication.Contains("1") && item == 6) || (model.UsersApplication.Contains("1") && item == 7) || (model.UsersApplication.Contains("1") && item == 8) || (model.UsersApplication.Contains("1") && item == 9))
                                    {
                                        var usertype = (UserRole)item;
                                        await _userManager.AddToRoleAsync(user.Id, usertype.ToString());
                                    }
                                    if ((model.UsersApplication.Contains("3") && item == 10) || (model.UsersApplication.Contains("3") && item == 2))
                                    {
                                        var usertype = (UserRole)item;
                                        await _userManager.AddToRoleAsync(user.Id, usertype.ToString());
                                    }

                                }

                            }
                            //create site user
                            var siteUser = new Model.EntityModel.SiteUser
                            {
                                AspNetUserId = user.Id,
                                FullName = model.FullName,
                                // UserType = model.UserType,
                                //UserImage=model.UserImage,
                                Status = model.Status,
                                Phone = model.Phone,
                                CreatedAt = DateTime.Now,
                                CreatedBy = _sessionManager.CurrentUser.FullName
                            };
                            _dbContext.SiteUsers.Add(siteUser);
                            _dbContext.SaveChanges();
                            if (model.UsersApplication != null && model.UsersApplication.Count > 0)
                            {
                                foreach (var item in model.UsersApplication)
                                {
                                    var usersapp = new Model.EntityModel.UsersApplication
                                    {
                                        UserId = siteUser.Id,
                                        AppId = Convert.ToInt16(item)
                                    };
                                    _dbContext.UsersApplications.Add(usersapp);

                                }
                            }
                            else
                            {
                                var app = _dbContext.Applications.AsQueryable();
                                foreach (var item in app)
                                {
                                    var usersapp = new Model.EntityModel.UsersApplication
                                    {
                                        UserId = siteUser.Id,
                                        AppId = Convert.ToInt16(item.Id)
                                    };
                                    _dbContext.UsersApplications.Add(usersapp);

                                }

                            }



                            _dbContext.SaveChanges();

                            //var userLocation= new Model
                            string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);


                            return new RegisterResponse
                            {
                                UserToken = user.Id,
                                // User = _sessionManager.GetUserDetails(user.UserName),
                            };

                        }
                        catch (Exception ex)
                        {
                            capturedException = ExceptionDispatchInfo.Capture(ex);
                        }
                        if (capturedException != null)
                        {

                            //delete just created user
                            await _userManager.DeleteAsync(user);



                            Elmah.ErrorSignal.FromCurrentContext().Raise(capturedException.SourceException);
                            return RegisterResponse.GetInvalidResponse(capturedException.SourceException.Message);
                        }
                    }
                    //return LoginResponse.GetInvalidResponse("Could not register");
                    return RegisterResponse.GetInvalidResponse(string.Join(", ", result.Errors));
                }

                RegisterResponse response = new RegisterResponse();

                response.AssignValidationErrors(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return response;
            }
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<Response> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //    if (string.IsNullOrEmpty(model.Email)) {
                //    return LoginResponse.GetInvalidResponse("User Email Must be Filled");
                //}
                //if (string.IsNullOrEmpty(model.Password))
                //{
                //    return LoginResponse.GetInvalidResponse("User Password Must be Filled");
                //}

                var userObject = _sessionManager.CurrentUser;

                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                if (result == SignInStatus.Success)
                {

                    var user = await _userManager.FindByNameAsync(model.Username);
                    var x = _dbContext.SiteUsers.FirstOrDefault(v => v.AspNetUser.UserName == model.Username);
                    if (!x.Status)
                    {
                        return LoginResponse.GetInvalidResponse("Your account is Deactivated.");
                    }

                    LoginResponse response = new LoginResponse
                    {
                        UserToken = user.Id,
                        User = _sessionManager.GetUserDetails(user.UserName),
                    };

                    //int IsEnable = _dbContext.Organizations.FirstOrDefault(x => x.Id == _sessionManager.CurrentUser.OrganizationId).Status;
                    //if (IsEnable==0)
                    //{
                    //    return LoginResponse.GetInvalidResponse(Messages.Invaliduserorpassword);
                    //}
                    return response;
                }
                return LoginResponse.GetInvalidResponse(Messages.Invaliduserorpassword);
            }

            LoginResponse errresponse = new LoginResponse();
            errresponse.AssignValidationErrors(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return errresponse;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<Response> Microsoftlogin(MicrosoftLoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.Email != null)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        return LoginResponse.GetInvalidResponse(Messages.Invaliduserorpassword);
                    }
                    LoginResponse response = new LoginResponse
                    {
                        UserToken = user.Id,
                        User = _sessionManager.GetUserDetails(user.UserName),
                    };
                    return response;
                }
                return LoginResponse.GetInvalidResponse(Messages.Invaliduserorpassword);
            }

            LoginResponse errresponse = new LoginResponse();
            errresponse.AssignValidationErrors(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return errresponse;
        }



        [HttpPost]
        public async Task<object> UsersPasswordResetRequest(ResetUserPasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model.Password))
            {
                return ResetPasswordResponse.GetInvalidResponse("Password Is Required");
            }
            if (string.IsNullOrEmpty(model.ConfirmPassword))
            {
                return ResetPasswordResponse.GetInvalidResponse("Confirm Password Is Required");
            }
            if (model.Password != model.ConfirmPassword)
            {

                return ResetPasswordResponse.GetInvalidResponse("Password Is Mismatched");
            }
            if (ModelState.IsValid)
            {
                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindByEmailAsync(model.Email);
                var code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                var resp = await userManager.ResetPasswordAsync(user.Id, code.ToString(), model.Password);
                return resp;
            }
            ResetPasswordResponse errresponse = new ResetPasswordResponse();
            errresponse.AssignValidationErrors(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return errresponse;

        }

        [HttpGet]
        public object getUserDetails([FromUri] GetUserDetailsRequest req)
        {

            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpPost]
        public async Task<object> UserInformationUpdate(UserInformationUpdateRequest req)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = _dbContext.SiteUsers.FirstOrDefault(x => x.Id == req.data.Id);

            var roles = await userManager.GetRolesAsync(user.AspNetUserId);

            foreach (var r in roles)
            {
                await userManager.RemoveFromRoleAsync(user.AspNetUserId, r);
            }


            bool IsSuccess = false;
            if (req.data.UserType.Count() > 0)
            {
                foreach (var item in req.data.UserType)
                {
                    if ((item == 1) || (req.data.UsersApplication.Contains(1) && item == 2) || (req.data.UsersApplication.Contains(2) && item == 3))
                    {
                        string role = Enum.Parse(typeof(UserRole), item.ToString()).ToString();
                        var result = await userManager.AddToRoleAsync(user.AspNetUserId, role);
                        if (result.Succeeded)
                        {
                            IsSuccess = true;
                        }
                    }
                    if ((req.data.UsersApplication.Contains(1) && item == 4) || (req.data.UsersApplication.Contains(1) && item == 5) || (req.data.UsersApplication.Contains(1) && item == 6) || (req.data.UsersApplication.Contains(1) && item == 7) || (req.data.UsersApplication.Contains(1) && item == 8) || (req.data.UsersApplication.Contains(1) && item == 9))
                    {
                        string role = Enum.Parse(typeof(UserRole), item.ToString()).ToString();
                        var result = await userManager.AddToRoleAsync(user.AspNetUserId, role);
                        if (result.Succeeded)
                        {
                            IsSuccess = true;
                        }
                    }
                    if ((req.data.UsersApplication.Contains(3) && item == 10) || (req.data.UsersApplication.Contains(3) && item == 2))
                    {
                        string role = Enum.Parse(typeof(UserRole), item.ToString()).ToString();
                        var result = await userManager.AddToRoleAsync(user.AspNetUserId, role);
                        if (result.Succeeded)
                        {
                            IsSuccess = true;
                        }
                    }

                }
            }
            else
            {
                IsSuccess = true;
            }



            UserInformationUpdateResponse resp = new UserInformationUpdateResponse();
            if (IsSuccess)
            {
                //req.DepartmentId = _sessionManager.CurrentUser.DefaultLocationId;
                resp = _requestExecutor.Execute(req);
            }
            // var   resp = _requestExecutor.Execute(req);      
            return resp;
        }
        //import Data



        public async Task<Response> ImportUsers(RegisterUserViewModel model)
        {


            if (ModelState.IsValid)
            {


                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                };





                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ExceptionDispatchInfo capturedException = null;
                    try
                    {
                        //var usertype = (UserRole)model.UserType;
                        //if (model.UserType > 0)
                        //{
                        //    await _userManager.AddToRoleAsync(user.Id, usertype.ToString());
                        //}


                        //create site user
                        var siteUser = new Model.EntityModel.SiteUser
                        {
                            AspNetUserId = user.Id,
                            FullName = model.FullName,
                            // UserType = model.UserType,
                            //UserImage = model.UserImage,
                        };

                        var sitedata = _dbContext.SiteUsers.Add(siteUser);

                        _dbContext.SaveChanges();
                        //var userLocation= new Model
                        string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);


                        // should now return https
                        try
                        {
                            var callbackUrl = Url.Link("Default", new { controller = "Account", userId = user.Id, code = code, action = "ResetPassword" });
                            await _userManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        return new RegisterResponse
                        {
                            UserToken = user.Id,
                            User = _sessionManager.GetUserDetails(user.UserName),
                        };

                    }
                    catch (Exception ex)
                    {
                        capturedException = ExceptionDispatchInfo.Capture(ex);
                    }
                    if (capturedException != null)
                    {

                        //delete just created user
                        await _userManager.DeleteAsync(user);



                        Elmah.ErrorSignal.FromCurrentContext().Raise(capturedException.SourceException);
                        return RegisterResponse.GetInvalidResponse(capturedException.SourceException.Message);
                    }
                }
                //return LoginResponse.GetInvalidResponse("Could not register");
                return RegisterResponse.GetInvalidResponse(string.Join(", ", result.Errors));
            }

            RegisterResponse response = new RegisterResponse();

            response.AssignValidationErrors(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return response;

        }
    }
}
