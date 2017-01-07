using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMastersClassLibrary.Logging
{
    public interface ILogger
    {
        void Trace<T>(string message);
        void Trace<T>(string message, Exception ex);
        void Trace<T>(string userId, string userName, string message);
        void Trace<T>(string userId, string userName, string message, Exception ex);

        void Debug<T>(string message);
        void Debug<T>(string message, Exception ex);
        void Debug<T>(string userId, string userName, string message);
        void Debug<T>(string userId, string userName, string message, Exception ex);

        void Info<T>(string message);
        void Info<T>(string message, Exception ex);
        void Info<T>(string userId, string userName, string message);
        void Info<T>(string userId, string userName, string message, Exception ex);

        void Warn<T>(string message);
        void Warn<T>(string message, Exception ex);
        void Warn<T>(string userId, string userName, string message);
        void Warn<T>(string userId, string userName, string message, Exception ex);

        void Error<T>(string message);
        void Error<T>(string message, Exception ex);
        void Error<T>(string userId, string userName, string message);
        void Error<T>(string userId, string userName, string message, Exception ex);

        void Fatal<T>(string message);
        void Fatal<T>(string message, Exception ex);
        void Fatal<T>(string userId, string userName, string message);
        void Fatal<T>(string userId, string userName, string message, Exception ex);

        void Log<T>(LogLevel level, string message);
        void Log<T>(LogLevel level, string message, Exception ex);
        void Log<T>(LogLevel level, string userId, string userName, string message);
        void Log<T>(LogLevel level, string userId, string userName, string message, Exception ex);


        bool IsTraceEnabled();
        bool IsDebugEnabled();
        bool IsInfoEnabled();
        bool IsWarningEnabled();
        bool IsErrorEnabled();
        bool IsFatalEnabled();
    }
}
