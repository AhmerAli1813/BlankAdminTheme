using DE.Infrastructure.Concept;
using DPWVessel.Web.Core.Session;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace DPWVessel.Web.Controllers
{
    public class HomeController : BaseController
    {
        private UserInformation _user;
        private readonly IRequestExecutor _requestExecutor;
        private readonly ISessionManager _sessionManager;

        public HomeController(IRequestExecutor requestExecutor, ISessionManager sessionManager)
        {
            _requestExecutor = requestExecutor;
            _sessionManager = sessionManager;
        }


        public ActionResult Index()
        {


            return View();
        }

        //Encrypt
        public string Encrypt(string Id)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(Id);
            byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
            byte[] key = { };
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes("P@ssw0rd");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //Decrypt
        public string Decrypt(string Id, string Password)
        {
            byte[] inputByteArray = new byte[Id.Length + 1];
            byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
            byte[] key = { };

            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(Password);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(Id);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}
