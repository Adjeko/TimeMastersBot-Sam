namespace TimeMasters.PortableClassLibrary.Logging
{
    public class Exception
    {
        //public string Data { get; set; }
        public string HelpLink { get; set; }
        public int HResult { get; set; }
        //public string InnerException { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        //public string TargetSite { get; set; }

        public Exception()
        {

        }

        public static Exception CreateFromSystemException(System.Exception sysex)
        {
            return new Exception
            {
                Message = sysex.Message,
                HResult = sysex.HResult,
                HelpLink = sysex.HelpLink,
                Source = sysex.Source,
                StackTrace = sysex.StackTrace,
            };
        }
    }
}