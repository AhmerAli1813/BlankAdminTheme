﻿using DE.Infrastructure.Concept;
using DE.Infrastructure.Security;
using DPWVessel.Model.Features.Users;
using DPWVessel.Model.Features.UsersApplications;
using DPWVessel.Web.Core.Session;
using System;

using System.Web.Http;

namespace DPWVessel.Web.Controllers.Api
{
    [Authorize]
    public class UsersApiController : BaseApiController
    {
        private readonly ISessionManager _sessionManager;

        public UsersApiController(IRequestExecutor requestExecutor, ISessionManager sessionManager)
        {
            _requestExecutor = requestExecutor;
            _sessionManager = sessionManager;

        }

        [HttpGet]
        public object GetUsersListRequest()
        {
            var req = new GetStudentsListRequest();
            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        [HttpGet]
        public object GetApplicationList()
        {
            var req = new GetApplicationsListRequest();
            var resp = _requestExecutor.Execute(req);

            return resp;
        }
        [HttpGet]
        public object GetUsersApplicationListRequest()
        {
            var req = new UsersApplicationsListRequest();
            var resp = _requestExecutor.Execute(req);
            return resp;
        }
        //Encrypt
        [HttpGet]
        public object Encrypt([FromUri] string Id)
        {
            return Encryption.Encrypt(Id.ToString());
        }
        //Decrypt
        [HttpGet]
        public object Decrypt([FromUri] string Id)
        {
            var x = Convert.ToInt32(Encryption.Decrypt(Id.Replace(" ", "+")));
            return x;
        }

        [HttpGet]
        public object Decryptstring([FromUri] string Id)
        {
            var x = Encryption.Decrypt(Id.Replace(" ", "+"));
            return x;
        }

        [HttpGet]
        public object DecryptDoc([FromUri] string Id)
        {
            var x = Encryption.Decrypt(Id.Replace(" ", "+"));
            return x;
        }

        //Encrypt
        //[HttpGet]
        //public string Encrypt(string Id)
        //{
        //    byte[] inputByteArray = Encoding.UTF8.GetBytes(Id);
        //    byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
        //    byte[] key = { };
        //    try
        //    {
        //        key = System.Text.Encoding.UTF8.GetBytes("P@ssw0rd");
        //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //        MemoryStream ms = new MemoryStream();
        //        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write);
        //        cs.Write(inputByteArray, 0, inputByteArray.Length);
        //        cs.FlushFinalBlock();
        //        return Convert.ToBase64String(ms.ToArray());
        //    }
        //    catch (Exception e)
        //    {
        //        return e.Message;
        //    }
        //}

        ////Decrypt
        //[HttpGet]
        //public string Decrypt(string Id, string Password)
        //{
        //    byte[] inputByteArray = new byte[Id.Length + 1];
        //    byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
        //    byte[] key = { };

        //    try
        //    {
        //        key = System.Text.Encoding.UTF8.GetBytes(Password);
        //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //        inputByteArray = Convert.FromBase64String(Id);
        //        MemoryStream ms = new MemoryStream();
        //        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write);
        //        cs.Write(inputByteArray, 0, inputByteArray.Length);
        //        cs.FlushFinalBlock();
        //        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        //        return encoding.GetString(ms.ToArray());
        //    }
        //    catch (Exception e)
        //    {
        //        return e.Message;
        //    }
        //}

    }

}