using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeMasters.Bot.Helpers.Luis
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class LuisIdentifierAttribute : Attribute
    {
        public string Value { get; }

        public LuisIdentifierAttribute(string value)
        {
            this.Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class LuisBuiltInIdentifierAttribute : Attribute
    {
        public string Value { get; }

        public LuisBuiltInIdentifierAttribute(string value)
        {
            this.Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class IsRequiredAttribute : Attribute
    {
        public bool Value { get; }

        public IsRequiredAttribute(bool value)
        {
            this.Value = value;
        }
    }
}