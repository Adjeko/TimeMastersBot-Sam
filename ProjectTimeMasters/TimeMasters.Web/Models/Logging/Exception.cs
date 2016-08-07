namespace TimeMasters.Web.Models.Logging
{
    public class Exception
    {
        public int ID { get; set; }
        public int EventsID { get; set; }
        //public string Data { get; set; }
        public string HelpLink { get; set; }
        public int HResult { get; set; }
        //public string InnerException { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        //public string TargetSite { get; set; }
        public Events Events { get; set; }

        public Exception()
        {

        }
    }
}