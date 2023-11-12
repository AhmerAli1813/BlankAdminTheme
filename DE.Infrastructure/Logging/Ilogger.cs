using System;

namespace DE.Infrastructure.Logging
{
    public interface ILogger
    {
        void Debug(string message, object additionalInfo = null);
        void Trace(string message, object additionalInfo = null);
        void Info(string message, object additionalInfo = null);
        void Warn(string message, object additionalInfo = null);
        void Error(string message);
        void Error(Exception ex);
        void Fatal(string message, object additionalInfo = null);
    }
}
