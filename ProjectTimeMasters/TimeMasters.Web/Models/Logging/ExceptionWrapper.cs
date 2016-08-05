namespace TimeMasters.Web.Models.Logging
{
    public class ExceptionWrapper
    {
        public int ID { get; set; }
        public string AsString { get; set; }
        public int Hresult { get; set; }
        public string TypeName { get; set; }
    }
}