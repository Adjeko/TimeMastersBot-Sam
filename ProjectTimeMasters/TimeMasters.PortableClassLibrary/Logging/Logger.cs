using System;
using MetroLog;
using MetroLog.Targets;


namespace TimeMasters.PortableClassLibrary.Logging
{
    public class Logger
    {
        private ILogManager _manager;

        private static Logger _logger;

        private Logger()
        {
            Uri u = new Uri("http://timemastersweb.azurewebsites.net/logs/addlog");

            LoggingConfiguration conf = new LoggingConfiguration();
            conf.AddTarget(LogLevel.Trace, LogLevel.Fatal, new JsonPostTarget(1, u));

            _manager = LogManagerFactory.CreateLogManager(conf);
        }

        public static Logger GetInstance()
        {
            if (_logger == null)
            {
                _logger = new Logger();
            }

            return _logger;
        }


        public void Trace<T>(string message, Exception ex = null)
        {
            Log<T>(LogLevel.Trace, message, ex);
        }

        public void Debug<T>(string message, Exception ex = null)
        {
            Log<T>(LogLevel.Debug, message, ex);
        }

        public void Info<T>(string message, Exception ex = null)
        {
            Log<T>(LogLevel.Info, message, ex);
        }

        public void Warn<T>(string message, Exception ex = null)
        {
            Log<T>(LogLevel.Warn, message, ex);
        }

        public void Error<T>(string message, Exception ex = null)
        {
            Log<T>(LogLevel.Error, message, ex);
        }

        public void Fatal<T>(string message, Exception ex = null)
        {
            Log<T>(LogLevel.Fatal, message, ex);
        }

        public void Log<T>(LogLevel logLevel, string message, Exception ex)
        {
            ILogger log = _manager.GetLogger<T>();

            if (log.IsEnabled(logLevel))
            {
                if (ex != null)
                {
                    log.Log(logLevel, message,
                        new Exception(ex.Message + "@" + ex.Source + "@" + ex.StackTrace + "@" + ex.HelpLink));
                }
                else
                {
                    log.Trace(message);
                }
            }
        }

        public bool IsTraceEnabled => _logger.IsTraceEnabled;
        public bool IsDebugEnabled => _logger.IsDebugEnabled;
        public bool IsInfoEnabled => _logger.IsInfoEnabled;
        public bool IsWarnEnabled => _logger.IsWarnEnabled;
        public bool IsErrorEnabled => _logger.IsErrorEnabled;
        public bool IsFatalEnabled => _logger.IsFatalEnabled;
    }
}