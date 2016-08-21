using System;

namespace TimeMasters.Web.Models.Logging
{
    public class Events
    {
        public int ID { get; set; }
        public int LogID { get; set; }

        public Exception Exception { get; set; }
        public ExceptionWrapper ExceptionWrapper { get; set; }

        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public int SequenceID { get; set; }
        public DateTime TimeStamp { get; set; }
        public Log Log { get; set; }

        public Events()
        {

        }
    }
}