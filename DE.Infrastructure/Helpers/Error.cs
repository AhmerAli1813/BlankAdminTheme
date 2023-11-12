﻿using System;

namespace DE.Infrastructure.Helpers
{
    [Serializable]
    public sealed class Error
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public Error()
        {
            TimeStamp = DateTime.Now;
        }

        public Error(string message)
            : this()
        {
            Message = message;
        }

        public Error(Exception ex)
            : this(ex.Message)
        {
            StackTrace = ex.StackTrace;
        }

        public override string ToString()
        {
            return Message + StackTrace;
        }
    }
}
