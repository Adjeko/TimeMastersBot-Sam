using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog;
using MetroLog.Targets;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Uri u = new Uri("http://localhost:41966/logmessages/Create");
            Uri u = new Uri("http://timemasterswebcore.azurewebsites.net/logmessages/create");

            //LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new JsonPostTarget(Int32.MaxValue, u));

            LoggingConfiguration conf = new LoggingConfiguration();
            conf.AddTarget(LogLevel.Trace, LogLevel.Fatal, new JsonPostTarget(1, u));

            ILogManager manager = LogManagerFactory.CreateLogManager(conf);

            ILogger log = manager.GetLogger<Program>();

            Console.WriteLine(log.IsInfoEnabled);
            Console.WriteLine(log.IsDebugEnabled);
            Console.WriteLine(log.IsErrorEnabled);
            Console.WriteLine(log.IsFatalEnabled);
            Console.WriteLine(log.IsTraceEnabled);
            Console.WriteLine(log.IsWarnEnabled);

            if (log.IsInfoEnabled)
            {
                log.Info("I can also format {0}.", "strings");
            }
            log.Trace("Trace Test");
            log.Info("Info", new Exception("Exception info test"));

            log.Error("Error Message !!!", "Help", "me", "pls", 5);

            Console.WriteLine("Logged dat shit");
            Console.ReadLine();
        }
    }
}
