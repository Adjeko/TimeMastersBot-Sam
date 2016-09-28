using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeMasters.Bot.Helpers.Luis
{
    public abstract class Information
    {
        public bool IsRequired { get; set; }

        public abstract bool IsSet();
    }
}