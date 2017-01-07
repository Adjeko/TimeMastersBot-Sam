using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMastersClassLibrary.Logging
{
    public class FileLogger : ILogger
    {
        private string _path;

        public FileLogger(string path)
        {
            _path = path;
        }
        
        public bool IsDebugEnabled() => true;
        public bool IsErrorEnabled() => true;
        public bool IsFatalEnabled() => true;
        public bool IsInfoEnabled() => true;
        public bool IsTraceEnabled() => true;
        public bool IsWarningEnabled() => true;

        public string GetAllText()
        {
            return File.ReadAllText(_path);
        }

        public string[] GetAllLines()
        {
            return File.ReadAllLines(_path);
        }

        public void Trace<T>(string message)
        {
            Log<T>(LogLevel.Trace, message);
        }

        public void Trace<T>(string message, Exception ex)
        {
            Log<T>(LogLevel.Trace, message, ex);
        }

        public void Trace<T>(string userId, string userName, string message)
        {
            Log<T>(LogLevel.Trace, userId, userName, message);
        }

        public void Trace<T>(string userId, string userName, string message, Exception ex)
        {
            Log<T>(LogLevel.Trace, userId, userName, message, ex);
        }

        public void Debug<T>(string message)
        {
            Log<T>(LogLevel.Debug, message);
        }

        public void Debug<T>(string message, Exception ex)
        {
            Log<T>(LogLevel.Debug, message, ex);
        }

        public void Debug<T>(string userId, string userName, string message)
        {
            Log<T>(LogLevel.Debug, userId, userName, message);
        }

        public void Debug<T>(string userId, string userName, string message, Exception ex)
        {
            Log<T>(LogLevel.Debug, userId, userName, message, ex);
        }

        public void Info<T>(string message)
        {
            Log<T>(LogLevel.Info, message);
        }

        public void Info<T>(string message, Exception ex)
        {
            Log<T>(LogLevel.Info, message, ex);
        }

        public void Info<T>(string userId, string userName, string message)
        {
            Log<T>(LogLevel.Info, userId, userName, message);
        }

        public void Info<T>(string userId, string userName, string message, Exception ex)
        {
            Log<T>(LogLevel.Info, userId, userName, message, ex);
        }

        public void Warn<T>(string message)
        {
            Log<T>(LogLevel.Warning, message);
        }

        public void Warn<T>(string message, Exception ex)
        {
            Log<T>(LogLevel.Warning, message, ex);
        }

        public void Warn<T>(string userId, string userName, string message)
        {
            Log<T>(LogLevel.Warning, userId, userName, message);
        }

        public void Warn<T>(string userId, string userName, string message, Exception ex)
        {
            Log<T>(LogLevel.Warning, userId, userName, message, ex);
        }

        public void Error<T>(string message)
        {
            Log<T>(LogLevel.Error, message);
        }

        public void Error<T>(string message, Exception ex)
        {
            Log<T>(LogLevel.Error, message, ex);
        }

        public void Error<T>(string userId, string userName, string message)
        {
            Log<T>(LogLevel.Error, userId, userName, message);
        }

        public void Error<T>(string userId, string userName, string message, Exception ex)
        {
            Log<T>(LogLevel.Error, userId, userName, message, ex);
        }

        public void Fatal<T>(string message)
        {
            Log<T>(LogLevel.Fatal, message);
        }

        public void Fatal<T>(string message, Exception ex)
        {
            Log<T>(LogLevel.Fatal, message, ex);
        }

        public void Fatal<T>(string userId, string userName, string message)
        {
            Log<T>(LogLevel.Fatal, userId, userName, message);
        }

        public void Fatal<T>(string userId, string userName, string message, Exception ex)
        {
            Log<T>(LogLevel.Fatal, userId, userName, message, ex);
        }

        public void Log<T>(LogLevel level, string message)
        {
            LogMessage tmp = new LogMessage {
                level = level,
                classType = typeof(T).FullName,
                time = DateTime.Now,
                userId = "",
                userName = "",
                message = message,
                exceptionMessage = "",
                stackTrace = "",
            };
            File.AppendAllLines(_path, new string[] { tmp.ToString() });
        }

        public void Log<T>(LogLevel level, string message, Exception ex)
        {
            LogMessage tmp = new LogMessage
            {
                level = level,
                classType = typeof(T).FullName,
                time = DateTime.Now,
                userId = "",
                userName = "",
                message = message,
                exceptionMessage = ex.Message,
                stackTrace = ex.StackTrace,
            };
            File.AppendAllLines(_path, new string[] { tmp.ToString() });
        }

        public void Log<T>(LogLevel level, string userId, string userName, string message)
        {
            LogMessage tmp = new LogMessage
            {
                level = level,
                classType = typeof(T).FullName,
                time = DateTime.Now,
                userId = userId,
                userName = userName,
                message = message,
                exceptionMessage = "",
                stackTrace = "",
            };
            File.AppendAllLines(_path, new string[] { tmp.ToString() });
        }

        public void Log<T>(LogLevel level, string userId, string userName, string message, Exception ex)
        {
            LogMessage tmp = new LogMessage
            {
                level = level,
                classType = typeof(T).FullName,
                time = DateTime.Now,
                userId = userId,
                userName = userName,
                message = message,
                exceptionMessage = ex.Message,
                stackTrace = ex.StackTrace,
            };
            File.AppendAllLines(_path, new string[] { tmp.ToString() });
        }
    }
}
