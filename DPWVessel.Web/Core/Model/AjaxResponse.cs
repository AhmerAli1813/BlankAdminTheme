﻿using System;

namespace DPWVessel.Web.Core.Model
{
    public class AjaxResponse
    {
        public static object ToResult(object payload = null, string message = "")
        {
            return new { Success = true, Payload = payload, Message = message };
        }

        public static object ToErrorResult(Exception e)
        {
            return new { Success = false, Payload = e, Message = e.Message };
        }
    }
}