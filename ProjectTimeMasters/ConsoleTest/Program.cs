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
            //Uri u = new Uri("http://localhost:12647/logs/send");
            Uri u = new Uri("http://timemastersweb.azurewebsites.net/logs/send");

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

            string tmp = Console.ReadLine();
            while (tmp != "quit")
            {
                //if (log.IsInfoEnabled)
                //{
                //    log.Info("I can also format {0}.", "strings");
                //}
                //log.Trace("Trace Test");
                //log.Info("Info", new Exception("Exception info test"));

                //log.Error("Error Message !!!", "Help", "me", "pls", 5);

                StackOverflowException soex;
                try
                {
                    throw new StackOverflowException();
                }
                catch(StackOverflowException ex)
                {
                    soex = ex;
                }

                Exception e = new Exception
                {
                    
                };


                log.Info("Test", soex);
                //log.Trace("sadads", soex);

                Console.WriteLine("logged");
                tmp = Console.ReadLine();
            }
            Console.WriteLine("Logged dat shit");
            Console.ReadLine();
        }
    }
}
