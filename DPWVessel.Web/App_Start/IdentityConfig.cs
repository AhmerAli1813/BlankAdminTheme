using DPWVessel.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DPWVessel.Web
{
    public class EmailService : IIdentityMessageService
    {
        public class IdentityMessage2
        {
            public virtual string Body { get; set; }
            public virtual string Destination { get; set; }
            public virtual string Subject { get; set; }
            public string FileData { get; set; }
            public string FileName { get; set; }
            public byte[] imageBytes { get; set; }
            public string CC { get; set; }


        }
        public Task SendAsyncWithAttechment(IdentityMessage2 message)
        {

            SmtpClient client = new SmtpClient();
            MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["sendFrom"], message.Destination, message.Subject, message.Body);
            msg.CC.Add(message.CC);
            // message.Destination = ConfigurationManager.AppSettings["EmailToAddress"];
            if (message.imageBytes != null && message.FileName != null)
            {
                //string pattern = @"data:([\w]*/([\w\.-]*));base64,";
                //var match = Regex.Match(message.FileData, pattern);
                //var cleanedString = message.FileData;
                //if (match.Success)
                //{
                //    string type = match.Groups[1].Value;
                //    string ext = match.Groups[2].Value;
                //    cleanedString = Regex.Replace(message.FileData, pattern, "");
                //}
                var ms = new MemoryStream(message.imageBytes, 0, message.imageBytes.Length);
                msg.Attachments.Add(new Attachment(ms, message.FileName));
            }

            msg.IsBodyHtml = true;


            //msg.IsBodyHtml = true;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return Task.FromResult(0);


            // Plug in your email service here to send an email.

        }

        public Task SendAsyncVessel(IdentityMessage message)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["sendGridUser"], System.Configuration.ConfigurationManager.AppSettings["sendGridPassword"]);
            //client.Credentials = credentials;
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(System.Configuration.ConfigurationManager.AppSettings["sendFrom"], message.Destination, message.Subject, message.Body);
            msg.IsBodyHtml = true;
            client.Send(msg);
            return Task.FromResult(0);

        }

        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["sendGridSmtpServer"], Convert.ToInt32(ConfigurationManager.AppSettings["sendGridPort"]));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["sendGridUser"], ConfigurationManager.AppSettings["sendGridPassword"]);
            client.Credentials = credentials;
            MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["FromEmailAddress"], message.Destination, message.Subject, message.Body);
            msg.IsBodyHtml = true;
            client.Send(msg);
            // Plug in your email service here to send an email.
            return System.Threading.Tasks.Task.FromResult(0);
        }
        public Task SendAsync1(IdentityMessage message)
        {
            SmtpClient client = new SmtpClient();
            MailMessage msg = new MailMessage(
                ConfigurationManager.AppSettings["FromEmailAddress"],
                message.Destination,
                message.Subject,
                message.Body);
            msg.IsBodyHtml = true;
            client.Send(msg);
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
        public System.Threading.Tasks.Task SendAsync2(MailMessage message)
        {
            //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["sendGridSmtpServer"], Convert.ToInt32(ConfigurationManager.AppSettings["sendGridPort"]));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["sendGridUser"], ConfigurationManager.AppSettings["sendGridPassword"]);
            client.Credentials = credentials;
            // MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["FromEmailAddress"], , message.Subject, message.Body);
            message.From = new MailAddress(ConfigurationManager.AppSettings["FromEmailAddress"]);
            message.IsBodyHtml = true;
            client.Send(message);
            // Plug in your email service here to send an email.
            return System.Threading.Tasks.Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return System.Threading.Tasks.Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            //var dbContext = context.GetAutofacLifetimeScope().Resolve<DPWVessel.Model.EntityModel.dpw_VesselEntities>();
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
