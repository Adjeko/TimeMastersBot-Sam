namespace TimeMasters.Web.Models.Logging
{
    public class Environment
    {
        public int ID { get; set; }
        public int MetroLogVersionID { get; set; }

        public string FxProfile { get; set; }
        public bool IsDebugging { get; set; }
        public string MachineName { get; set; }
        public virtual MetroLogVersion MetroLogVersion { get; set; }
        public string SessionId { get; set; }
    }
}