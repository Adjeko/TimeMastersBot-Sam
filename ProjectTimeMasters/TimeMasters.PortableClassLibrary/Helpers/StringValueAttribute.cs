using System;

namespace TimeMasters.PortableClassLibrary.Helpers
{
    public class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}