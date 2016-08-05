namespace TimeMasters.Web.Models.Logging
{
    public class ExceptionWrapper
    {
        public int ID { get; set; }
        public int EventsID { get; set; }
        public string AsString { get; set; }
        public int Hresult { get; set; }
        public string TypeName { get; set; }
        public virtual Events Events { get; set; }

        public ExceptionWrapper()
        {

        }
    }
}