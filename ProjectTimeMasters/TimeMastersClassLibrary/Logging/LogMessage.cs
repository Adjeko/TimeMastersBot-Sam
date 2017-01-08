using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TimeMastersClassLibrary.Logging
{
    public class LogMessage
    {
        public LogLevel level;
        public string classType;
        public DateTime time;
        public string userId;
        public string userName;
        public string message;
        public string payload;
        public string exceptionMessage;
        public string stackTrace;

        public override string ToString()
        {
            return Regex.Replace(Newtonsoft.Json.JsonConvert.SerializeObject(this), @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", String.Empty);
        }

        public static LogMessage GetFromJSONString(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<LogMessage>(json);
        }
    }
}
