namespace TimeMasters.Web.Models.Logging
{
    public class Log
    {
        public int ID { get; set; }
        public int EnvironmentID { get; set; }
        public int EventsID { get; set; }


        public virtual Environment Environment { get; set; }
        public virtual Events Events { get; set; }
    }
}