using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMastersClassLibrary.Logging
{
    public class LoggerFactory
    {
        private static FileLogger _fileLogger;

        public static FileLogger GetFileLogger(string s)
        {
            if (_fileLogger == null)
            {
                _fileLogger = new FileLogger(s);
            }
            return _fileLogger;
        }
        

    }
}
