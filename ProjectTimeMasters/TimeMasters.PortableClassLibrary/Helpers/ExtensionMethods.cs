using System;
using System.Reflection;

namespace TimeMasters.PortableClassLibrary.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();

            FieldInfo fieldInfo = type.GetRuntimeField(value.ToString());

            StringValueAttribute[] attribs =
                fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            //return the first
            return attribs.Length > 0 ? attribs[0].Value : null;
        }
    }
}