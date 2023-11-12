using DE.Infrastructure.Concept;
using DE.Infrastructure.Memory;
using DPWVessel.Model.EntityModel;
using DPWVessel.Web.Core.Session;
using DPWVessel.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DPWVessel.Web.Controllers
{

    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IRequestExecutor _requestExecutor;
        private IAppBuilder IApp;
        private SessionManager _sessonManager;
        private IMemoryCacher _cacher;
        public static bool _IsLogIn;
        private dpw_VesselEntities _dbcontext;
        public AccountController(dpw_VesselEntities dbcontext, IMemoryCacher cacher, IRequestExecutor requestExecutor)
        {
            _requestExecutor = requestExecutor;
            _cacher = cacher;
            _dbcontext = dbcontext;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAppBuilder _iapp, SessionManager _SManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            IApp = _iapp;
            _sessonManager = _SManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {

                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        LoginViewModel LoginData = new LoginViewModel();
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            //  ViewBag.ReturnUrl = "/";
            ViewBag.ReturnUrl = returnUrl;

            //LoginData.ShedList = GetShedsList();
            return View(LoginData);
        }
        //List<ShedListData> GetShedsList()
        //{
        //    var req = new GetShedsListRequest();
        //    var resp = _requestExecutor.Execute(req);
        //    return resp.ShedList;
        //}
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                //model.ShedList = GetShedsList();
                return View(model);
            }
            //_cacher.Add("ShedId", model.ShedId, 1);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {

                case SignInStatus.Success:
                    // _dbcontext.SiteUsers.FirstOrDefault(x => x.AspNetUser.UserName == model.Username).ShedId = model.ShedId;
                    //_dbcontext.SaveChanges();
                    var x = _dbcontext.SiteUsers.FirstOrDefault(v => v.AspNetUser.UserName == model.Username);
                    //switch (x.UsersApplications.Count() != 0)
                    //{
                    //    case x.UsersApplications.:
                    //        ModelState.AddModelError("", "Invalid login attempt.");
                    //        TempData["CustomError"] = "Your is account deactivated.";
                    //        return RedirectToAction("login", "Account");

                    //    default:
                    //        return View();
                    //}

                    if (!x.Status)
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        TempData["CustomError"] = "Your account is deactivated.";
                        return RedirectToAction("login", "Account");
                    }

                    if (x.UsersApplications.Count() != 0)
                    {
                        bool dpw_jip = false;
                        bool dpw_ops = false;
                        bool dpw_DWH = false;
                        foreach (var item in x.UsersApplications)
                        {
                            if (item.Application.AppCode == "DPW_JIP")
                            {
                                dpw_jip = true;
                            }
                            if (item.Application.AppCode == "DPW_OPS")
                            {
                                dpw_ops = true;
                            }
                            if (item.Application.AppCode == "DPW_DWH")
                            {
                                dpw_DWH = true;
                            }
                        }

                        //Check and  Redirect According to Project
                        if (dpw_ops == true)
                        {
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            return RedirectToAction("Index", "VesselFormOps");

                        }
                        else
                        {
                            if (dpw_jip == true)
                            {
                                if (Url.IsLocalUrl(returnUrl))
                                {
                                    return Redirect(returnUrl);
                                }
                                return RedirectToAction("VistIndex", "JIP");

                            }
                            else
                            {

                                if (dpw_DWH == true)
                                {
                                    if (Url.IsLocalUrl(returnUrl))
                                    {
                                        return Redirect(returnUrl);
                                    }
                                    return RedirectToAction("Index", "DWH");
                                }
                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        TempData["CustomError"] = "You donot have any Project Access.";
                        return RedirectToAction("login", "Account");
                    }
                    switch (x.Status)
                    {
                        case false:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            TempData["CustomError"] = "Your is account deactivated.";
                            return RedirectToAction("login", "Account");

                        default:
                            return View();
                    }


                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    //model.ShedList = GetShedsList();
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register

        public ActionResult RegisterUser()
        {

            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    // return View("ForgotPasswordConfirmation");
                    return Content("Invalid Email");
                }
                else
                {
                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    model.callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    try
                    {
                        string emailBody = Helpers.EmailHelper.RenderViewToString("Empty", "~/Views/Emails/ResetPasswordEmail.cshtml", model);

                        var emailService = new EmailService();
                        var subject = "DPWVessel Reset Password";

                        Microsoft.AspNet.Identity.IdentityMessage message = new Microsoft.AspNet.Identity.IdentityMessage()
                        {
                            Subject = subject,
                            Body = emailBody,
                            Destination = model.Email
                        };
                        await emailService.SendAsync(message);
                        //await _userManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    }
                    catch (Exception ep)
                    {
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ep);
                    }
                    //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ///return RedirectToAction("ResetPasswordConfirmation", "Account");
                return Content("Invalid User");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            //var user = _sessonManager.CurrentUser.FullName;
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)

        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            var loginInfo1 = AuthenticationManager.User;
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            var useremail = loginInfo.ExternalIdentity?.FindFirst("preferred_username")?.Value;
            var user = await UserManager.FindByEmailAsync(useremail);

            if (user != null)
            {

                await SignInManager.SignInAsync(user, true, true);
            }
            return RedirectToLocal(returnUrl);
            // Sign in the user with this external login provider if the user already has a login
            //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
            //    case SignInStatus.Failure:
            //    default:
            //        // If the user does not have an account, then prompt the user to create an account
            //        ViewBag.ReturnUrl = returnUrl;
            //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            //}
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            _cacher.Remove(User.Identity.Name);
            // _cacher.Remove("ShedId");
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        public ActionResult ProfileView()
        {
            return View();
        }


        public ActionResult Edit()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [AllowAnonymous]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                var s = _sessonManager.CurrentUser;
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        //SSO WEB ..............................................

        public void MicrosoftSignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/Account/MicrosoftSignResp" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public async Task<ActionResult> MicrosoftSignResp(string returnUrl)
        {
            var loginInfo = AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            var useremail = loginInfo.Result.ExternalIdentity?.FindFirst("preferred_username")?.Value;
            var user = await UserManager.FindByEmailAsync(useremail);

            if (user != null)
            {

                await SignInManager.SignInAsync(user, true, true);
            }
            return RedirectToLocal(returnUrl);
        }
        public void SignOut1()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);
            HttpContext.Session.Clear();

        }

        //SSO MBL..............................................


        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/Account/SignResp" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public object SignResp()
        {
            var loginInfo = AuthenticationManager.GetExternalLoginInfoAsync();
            var loginInfo1 = AuthenticationManager.User;
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            if (loginInfo == null)
            {
                return false;
            }
            var useremail = loginInfo.Result.ExternalIdentity?.FindFirst("preferred_username")?.Value;

            return Redirect("EmailView?Email=" + useremail);
        }

        public ActionResult EmailView(string Email)
        {
            return View();
        }


    }
}