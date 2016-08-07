namespace TimeMasters.Web.Models.Logging
{
    public class Log
    {
        public int ID { get; set; }

        public Environment Environment { get; set; }
        public Events[] Events { get; set; }

        public Log()
        {

        }
    }
}