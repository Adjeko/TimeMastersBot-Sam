using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace TimeMasters.Bot.Helpers.Luis
{
    public class LuisInformation<T> : Information
    {
        public string LuisIdentifier { get; set; }

        public string LuisBuiltinIdentifier { get; set; }

        public T Entity { get; set; }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public LuisInformation()
        {
            
        }

        public override void SetEntitiy(object value)
        {
            if(value != null)
            Entity = (T) value;
        }

        public override bool IsSet()
        {
            return Entity != null;
        }

        public override Type Is()
        {
            return typeof(T);
        }

        public override string ToString()
        {
            return LuisIdentifier;
        }
    }
}