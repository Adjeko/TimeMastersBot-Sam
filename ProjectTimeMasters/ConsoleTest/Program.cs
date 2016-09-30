using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog;
using MetroLog.Targets;
using Microsoft.Bot.Builder.Luis.Models;
using TimeMasters.PortableClassLibrary.Logging;
using TimeMasters.Bot.Helpers.Luis;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //LoggerTest();
            LuisInformationTester();
        }

        static void LuisInformationTester()
        {
            InformationManager infoManager = new InformationManager();
            infoManager.AddEntity(new LuisInformation<string>{ IsRequired = true, LuisIdentifier = "Calendar::Title", LuisBuiltinIdentifier = ""});
            infoManager.AddEntity(new LuisInformation<DateTime> { IsRequired = false, LuisIdentifier = "Calendar::StartDate", LuisBuiltinIdentifier = "builtin.datetime.date" });
            infoManager.AddEntity(new LuisInformation<LuisResult>() {IsRequired = false, LuisIdentifier = "Shit::RandomNumber", LuisBuiltinIdentifier = ""});

            List<IntentRecommendation> intentList = new List<IntentRecommendation>();
            intentList.Add(new IntentRecommendation() {Intent = "CreateCalendarEntry", Score = 0.9971466 });
            intentList.Add(new IntentRecommendation() { Intent = "UpdateCalendarEntry", Score = 0.018504601 });
            intentList.Add(new IntentRecommendation() { Intent = "None", Score = 0.000772698 });
            intentList.Add(new IntentRecommendation() { Intent = "DeleteCalendarEntry", Score = 0.0005007981 });

            List<EntityRecommendation> entityList = new List<EntityRecommendation>();
            entityList.Add(new EntityRecommendation() {Entity = "friday", Type = "Calendar::StartDate", StartIndex = 11, EndIndex = 16, Score = 0.9830753});
            entityList.Add(new EntityRecommendation() { Entity = "gym", Type = "Calendar::Title", StartIndex = 4, EndIndex = 6, Score = 0.998410761});
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("date", "XXXX-WXX-5");
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "builtin.datetime.date", StartIndex = 11, EndIndex = 16, Resolution = dic});

            infoManager.ProcessResult(new LuisResult("add gym on friday", intentList, entityList));
            Console.WriteLine($"Required: {infoManager.IsRequiredAvailable()} + All: {infoManager.IsAllAvailable()}");
            Console.WriteLine(infoManager.GetMissingInformation());
            Console.Read();
        }

        static void LoggerTest()
        {
            //Uri u = new Uri("http://localhost:12647/logs/send");
            //Uri u = new Uri("http://timemastersweb.azurewebsites.net/logs/addlog");

            ////LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new JsonPostTarget(Int32.MaxValue, u));

            //LoggingConfiguration conf = new LoggingConfiguration();
            //conf.AddTarget(LogLevel.Trace, LogLevel.Fatal, new JsonPostTarget(1, u));

            //ILogManager manager = LogManagerFactory.CreateLogManager(conf);

            //ILogger log = manager.GetLogger<Program>();

            //Console.WriteLine(log.IsInfoEnabled);
            //Console.WriteLine(log.IsDebugEnabled);
            //Console.WriteLine(log.IsErrorEnabled);
            //Console.WriteLine(log.IsFatalEnabled);
            //Console.WriteLine(log.IsTraceEnabled);
            //Console.WriteLine(log.IsWarnEnabled);

            Logger log = new Logger();


            string tmp = Console.ReadLine();
            while (tmp != "quit")
            {

                StackOverflowException soex;
                try
                {
                    throw new StackOverflowException();
                }
                catch (StackOverflowException ex)
                {
                    soex = ex;
                }

                Exception e = new Exception
                {
                    
                };


                log.Info<Program>("Test", soex);

                Console.WriteLine("logged");
                tmp = Console.ReadLine();
            }
            Console.WriteLine("Logged dat shit");
            Console.ReadLine();
        }
    }
}
