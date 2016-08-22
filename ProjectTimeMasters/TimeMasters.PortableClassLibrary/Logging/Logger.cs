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
            Uri u = new Uri("http://timemastersweb.azurewebsites.net/logs/addlog");

        }

        public static Logger GetInstance()
        {
            if (_logger == null)
            {
                _logger = new Logger();
            }

            return _logger;
        }

        public string TestRestSharp()
        {
            var client = new RestClient("http://timemastersweb.azurewebsites.net");

            var request = new RestRequest("/logs/addLog", Method.POST);
            request.AddJsonBody(GetLog());

            //IRestResponse response;

            Task<IRestResponse> asyncHandle = client.Execute(request);

            return asyncHandle.Result.Content;

        }

        public void SetClient()
        {
            _client = new RestClient("http://timemastersweb.azurewebsites.net");
        }

        public void SetRequest()
        {
            _request = new RestRequest("/logs/addLog", Method.POST);
            _request.AddJsonBody(GetLog());
        }

        public string ExecuteRequest()
        {
            _asyncHandle = _client.Execute(_request);
            _asyncHandle.Start();
            return _asyncHandle.Status.ToString();
        }

        public Log GetLog()
        {
            return new Log()
            {
                Environment = new Environment
                {
                    FxProfile = ".NET Core",
                    IsDebugging = false,
                    MachineName = "Adjeko_Desktop",
                    SessionId = "1234",
                    MetroLogVersion = new MetroLogVersion
                    {
                        Revision = 1,
                        Build = 2,
                        Major = 3,
                        MajorRevision = 4,
                        Minor = 5,
                        MinorRevision = 6
                    }
                },
                Events = new Events
                {
                    Logger = "Bot2",
                    Level = "Experimental",
                    Message = "RestSharp for the win",
                    SequenceID = 2,
                    TimeStamp = DateTime.Now,
                    Exception = new TimeMasters.PortableClassLibrary.Logging.Exception
                    {
                        Message = "you just fucked up",
                        HelpLink = "fuckyoudotcom",
                        StackTrace = "where you fucked up",
                        HResult = 0,
                        Source = "yo mama"
                    },
                    ExceptionWrapper = new ExceptionWrapper
                    {
                        AsString = "ExceptionWrapper fuck you",
                        Hresult = 1321214,
                        TypeName = "FuckYouException"
                    }
                }
            };
        }


        //public void Trace<T>(string message, Exception ex = null)
        //{
        //    Log<T>(LogLevel.Trace, message, ex);
        //}

        //public void Debug<T>(string message, Exception ex = null)
        //{
        //    Log<T>(LogLevel.Debug, message, ex);
        //}

        //public void Info<T>(string message, Exception ex = null)
        //{
        //    Log<T>(LogLevel.Info, message, ex);
        //}

        //public void Warn<T>(string message, Exception ex = null)
        //{
        //    Log<T>(LogLevel.Warn, message, ex);
        //}

        //public void Error<T>(string message, Exception ex = null)
        //{
        //    Log<T>(LogLevel.Error, message, ex);
        //}

        //public void Fatal<T>(string message, Exception ex = null)
        //{
        //    Log<T>(LogLevel.Fatal, message, ex);
        //}

        //public void Log<T>(LogLevel logLevel, string message, Exception ex)
        //{
        //    ILogger log = _manager.GetLogger<T>();

        //    if (log.IsEnabled(logLevel))
        //    {
        //        if (ex != null)
        //        {
        //            log.Log(logLevel, message,
        //                new Exception(ex.Message + "@" + ex.Source + "@" + ex.StackTrace + "@" + ex.HelpLink));
        //        }
        //        else
        //        {
        //            log.Trace(message);
        //        }
        //    }
        //}

        //public bool IsTraceEnabled => _logger.IsTraceEnabled;
        //public bool IsDebugEnabled => _logger.IsDebugEnabled;
        //public bool IsInfoEnabled => _logger.IsInfoEnabled;
        //public bool IsWarnEnabled => _logger.IsWarnEnabled;
        //public bool IsErrorEnabled => _logger.IsErrorEnabled;
        //public bool IsFatalEnabled => _logger.IsFatalEnabled;
    }
}