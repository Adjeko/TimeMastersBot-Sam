using System;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;


namespace TimeMasters.PortableClassLibrary.Logging
{
    public class Logger
    {
        private static Logger _logger;
        private RestClient _client;
        private RestRequest _request;
        private Task<IRestResponse> _asyncHandle;

        private Logger()
        {
            //Uri u = new Uri("http://timemastersweb.azurewebsites.net/logs/addlog");

            _client = new RestClient("http://timemastersweb.azurewebsites.net");
        }

        public static Logger GetInstance()
        {
            if (_logger == null)
            {
                _logger = new Logger();
            }

            return _logger;
        }

        public Log GetLog()
        {
            return new Log()
            {
                Environment = new Environment
                {
                    FxProfile = "YOUR FRAMEWORK",
                    IsDebugging = false,
                    MachineName = "YOUR COMPUTER",
                    SessionId = Guid.NewGuid().ToString(),
                    MetroLogVersion = new MetroLogVersion
                    {
                        Revision = 1,
                        Build = 1,
                        Major = 1,
                        MajorRevision = 1,
                        Minor = 1,
                        MinorRevision = 1
                    }
                },
                Events = new Events
                {
                    Logger = "LOGGER NOT SET",
                    Level = "LOGLEVEL NOT SET",
                    Message = "MESSAGE NOT SET",
                    SequenceID = 1,
                    TimeStamp = DateTime.Now,
                    Exception = new TimeMasters.PortableClassLibrary.Logging.Exception
                    {
                        Message = "",
                        HelpLink = "",
                        StackTrace = "",
                        HResult = 0,
                        Source = ""
                    },
                    ExceptionWrapper = new ExceptionWrapper
                    {
                        AsString = "",
                        Hresult = 0,
                        TypeName = ""
                    }
                }
            };
        }


        public void Trace<T>(string message, System.Exception ex = null)
        {
            Log<T>(LogLevel.Trace, message, ex);
        }

        public void Debug<T>(string message, System.Exception ex = null)
        {
            Log<T>(LogLevel.Debug, message, ex);
        }

        public void Info<T>(string message, System.Exception ex = null)
        {
            Log<T>(LogLevel.Info, message, ex);
        }

        public void Warn<T>(string message, System.Exception ex = null)
        {
            Log<T>(LogLevel.Warning, message, ex);
        }

        public void Error<T>(string message, System.Exception ex = null)
        {
            Log<T>(LogLevel.Error, message, ex);
        }

        public void Fatal<T>(string message, System.Exception ex = null)
        {
            Log<T>(LogLevel.Fatal, message, ex);
        }

        public void Log<T>(LogLevel logLevel, string message, System.Exception ex)
        {
            Log log = GetLog();
            if (ex != null)
            {
                log.Events.Exception = Exception.CreateFromSystemException(ex);
                log.Events.ExceptionWrapper = ExceptionWrapper.CreateFromSystemException(ex);
            }
            
            log.Events.Logger = typeof(T).FullName;
            log.Events.Message = message;
            log.Events.Level = logLevel.ToString();


            var request = new RestRequest("/logs/addLog", Method.POST);
            request.AddJsonBody(log);

            Task.Run(() => _client.Execute(request));
        }

        public bool IsTraceEnabled => true;
        public bool IsDebugEnabled => true;
        public bool IsInfoEnabled => true;
        public bool IsWarningEnabled => true;
        public bool IsErrorEnabled => true;
        public bool IsFatalEnabled => true;
    }
}