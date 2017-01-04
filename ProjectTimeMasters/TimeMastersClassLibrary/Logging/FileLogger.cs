using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMastersClassLibrary.Logging
{
    public class FileLogger : ILogger
    {
        public void Debug<T>(string message)
        {
            Log<T>(LogLevel.Debug, message);
        }

        public void Debug<T>(string message, string exceptionMessage, string stackTrace)
        {
            Log<T>(LogLevel.Debug, message, exceptionMessage, stackTrace);
        }

        public void Error<T>(string message)
        {
            Log<T>(LogLevel.Error, message);
        }

        public void Error<T>(string message, string exceptionMessage, string stackTrace)
        {
            Log<T>(LogLevel.Error, message, exceptionMessage, stackTrace);
        }

        public void Fatal<T>(string message)
        {
            Log<T>(LogLevel.Fatal, message);
        }

        public void Fatal<T>(string message, string exceptionMessage, string stackTrace)
        {
            Log<T>(LogLevel.Fatal, message, exceptionMessage, stackTrace);
        }

        public void Info<T>(string message)
        {
            Log<T>(LogLevel.Info, message);
        }

        public void Info<T>(string message, string exceptionMessage, string stackTrace)
        {
            Log<T>(LogLevel.Info, message, exceptionMessage, stackTrace);
        }
        
        public void Trace<T>(string message)
        {
            Log<T>(LogLevel.Trace, message);
        }

        public void Trace<T>(string message, string exceptionMessage, string stackTrace)
        {
            Log<T>(LogLevel.Trace, message, exceptionMessage, stackTrace);
        }

        public void Warn<T>(string message)
        {
            Log<T>(LogLevel.Warning, message);
        }

        public void Warn<T>(string message, string exceptionMessage, string stackTrace)
        {
            Log<T>(LogLevel.Warning, message, exceptionMessage, stackTrace);
        }

        public void Log<T>(LogLevel level, string message)
        {
           
        }

        public void Log<T>(LogLevel level, string message, string exceptionMessage, string stackTrace)
        {
            
        }

        public bool IsDebugEnabled() => true;
        public bool IsErrorEnabled() => true;
        public bool IsFatalEnabled() => true;
        public bool IsInfoEnabled() => true;
        public bool IsTraceEnabled() => true;
        public bool IsWarningEnabled() => true;
    }
}
