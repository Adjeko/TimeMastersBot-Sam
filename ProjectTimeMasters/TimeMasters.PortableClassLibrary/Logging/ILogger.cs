using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMasters.PortableClassLibrary.Logging
{
    public interface ILogger
    {
        void Trace<T>(string message);
        void Trace<T>(string message, string exceptionMessage, string stackTrace);

        void Debug<T>(string message);
        void Debug<T>(string message, string exceptionMessage, string stackTrace);

        void Info<T>(string message);
        void Info<T>(string message, string exceptionMessage, string stackTrace);

        void Warn<T>(string message);
        void Warn<T>(string message, string exceptionMessage, string stackTrace);

        void Error<T>(string message);
        void Error<T>(string message, string exceptionMessage, string stackTrace);

        void Fatal<T>(string message);
        void Fatal<T>(string message, string exceptionMessage, string stackTrace);

        void Log<T>(LogLevel level, string message);
        void Log<T>(LogLevel level, string message, string exceptionMessage, string stackTrace);


        bool IsTraceEnabled();
        bool IsDebugEnabled();
        bool IsInfoEnabled();
        bool IsWarningEnabled();
        bool IsErrorEnabled();
        bool IsFatalEnabled();
    }
}
