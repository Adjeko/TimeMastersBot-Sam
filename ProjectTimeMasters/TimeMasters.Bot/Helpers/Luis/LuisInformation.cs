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

        public string LuisBuildtinIdentifier { get; set; }

        public T Entity { get; set; }

        //public bool isRequired { get; set; }

        public override bool IsSet()
        {
            return Entity != null;
        }
    }
}