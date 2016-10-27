namespace TimeMasters.PortableClassLibrary.Logging
{ 
    public class ExceptionWrapper
    {
        public string AsString { get; set; }
        public int Hresult { get; set; }
        public string TypeName { get; set; }

        public ExceptionWrapper()
        {

        }

        public static ExceptionWrapper CreateFromSystemException(System.Exception sysex)
        {
            return new ExceptionWrapper
            {
                AsString = sysex.ToString(),
                Hresult = sysex.HResult,
                TypeName = sysex.GetType().FullName
            };
        }
    }
}