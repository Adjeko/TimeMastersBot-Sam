using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeMasters.WebCore.Models
{
    public class LogMessageRepository : ILogMessageRepository
    {
        private static ConcurrentDictionary<string, LogMessage> _logMessages = new ConcurrentDictionary<string, LogMessage>();

        public LogMessageRepository()
        {
            Add(new LogMessage { Message = "testMessage" });
        }

        public void Add(LogMessage logMessage)
        {
            logMessage.Key = Guid.NewGuid().ToString();
            _logMessages[logMessage.Key] = logMessage;
        }

        public LogMessage Find(string key)
        {
            LogMessage log;
            _logMessages.TryGetValue(key, out log);
            return log;
        }

        public IEnumerable<LogMessage> GetAll()
        {
            return _logMessages.Values;
        }

        public LogMessage Remove(string key)
        {
            LogMessage log;
            _logMessages.TryGetValue(key, out log);
            _logMessages.TryRemove(key, out log);
            return log;
        }

        public void Update(LogMessage logMessage)
        {
            _logMessages[logMessage.Key] = logMessage;
        }
    }
}
