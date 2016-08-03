using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeMasters.WebCore.Models
{
    public interface ILogMessageRepository
    {
        void Add(LogMessage logMessage);
        IEnumerable<LogMessage> GetAll();
        LogMessage Find(string key);
        LogMessage Remove(string key);
        void Update(LogMessage logMessage);
    }
}
