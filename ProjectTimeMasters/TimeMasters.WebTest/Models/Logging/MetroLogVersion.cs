namespace TimeMasters.WebTest.Models.Logging
{
    public class MetroLogVersion
    {
        public int ID { get; set; }
        public int EnvironmentID { get; set; }
        public int Build { get; set; }
        public int Major { get; set; }
        public int MajorRevision { get; set; }
        public int Minor { get; set; }
        public int MinorRevision { get; set; }
        public int Revision { get; set; }
        public virtual Environment Environment { get; set; }


        public MetroLogVersion()
        {

        }
    }
}