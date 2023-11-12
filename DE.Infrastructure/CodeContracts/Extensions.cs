﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DE.Infrastructure.CodeContracts
{
    public static class Extensions
    {
        public static string ToJson<T>(this T value)
        {
            var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.SerializeObject(value, settings);
        }

        public static IEnumerable<Error> GenerateErrorList(this Exception ex)
        {
            var errors = new List<Error>();
            var currentException = ex;

            while (currentException != null)
            {
                errors.Add(new Error(currentException));
                currentException = currentException.InnerException;
            }

            return errors;
        }
    }
}
