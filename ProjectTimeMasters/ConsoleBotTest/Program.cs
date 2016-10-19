using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using TimeMasters.Bot.Helpers.Luis;
using TimeMasters.PortableClassLibrary.Helpers;


namespace ConsoleBotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth * 2, Console.WindowHeight * 2);
            //ReflectionsTest();
            LuisInformationTester();
            
        }

        public static void LuisInformationTester()
        {
            InformationManager<Calendar> infoManager = new InformationManager<Calendar>();
            //infoManager.AddEntity(new LuisInformation<string> { IsRequired = true, LuisIdentifier = "Calendar::Title", LuisBuiltinIdentifier = "" });
            //infoManager.AddEntity(new LuisInformation<DateTime> { IsRequired = false, LuisIdentifier = "Calendar::StartDate", LuisBuiltinIdentifier = "builtin.datetime.date" });
            //infoManager.AddEntity(new LuisInformation<LuisResult>() { IsRequired = false, LuisIdentifier = "Shit::RandomNumber", LuisBuiltinIdentifier = "" });

            List<IntentRecommendation> intentList = new List<IntentRecommendation>();
            intentList.Add(new IntentRecommendation() { Intent = "CreateCalendarEntry", Score = 0.9971466 });
            intentList.Add(new IntentRecommendation() { Intent = "UpdateCalendarEntry", Score = 0.018504601 });
            intentList.Add(new IntentRecommendation() { Intent = "None", Score = 0.000772698 });
            intentList.Add(new IntentRecommendation() { Intent = "DeleteCalendarEntry", Score = 0.0005007981 });

            List<EntityRecommendation> entityList = new List<EntityRecommendation>();
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "Calendar::StartDate", StartIndex = 11, EndIndex = 16, Score = 0.9830753 });
            entityList.Add(new EntityRecommendation() { Entity = "sunday", Type = "Calendar::StartDate", StartIndex = 22, EndIndex = 27, Score = 0.9144727 });
            entityList.Add(new EntityRecommendation() { Entity = "gym", Type = "Calendar::Title", StartIndex = 4, EndIndex = 6, Score = 0.998410761 });
            entityList.Add(new EntityRecommendation() { Entity = "17:00", Type = "Calendar::StartTime", StartIndex = 32, EndIndex = 45, Score = 0.9840954 });
            entityList.Add(new EntityRecommendation() { Entity = "19:00", Type = "Calendar::EndTime", StartIndex = 41, EndIndex = 36, Score = 0.853437364 });
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("date", "XXXX-WXX-5");
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "builtin.datetime.date", StartIndex = 11, EndIndex = 16, Resolution = dic });

            infoManager.ProcessResult(new LuisResult("add gym on friday and sunday at 17:00 to 19:00", intentList, entityList));
            //Console.WriteLine($"Required: {infoManager.IsOneRequiredAvailable()} + All: {infoManager.IsOneAllAvailable()}");
            //Console.WriteLine(infoManager.GetMissingInformation());
            Console.WriteLine(infoManager.GetNextMissingInformation());
            Console.ReadLine();

            var result = infoManager.GetFinishedEntries();

            Console.ReadLine();
        }

        public static void ReflectionsTest()
        {
            Calendar calend = new Calendar() { Title = "Test" };
            Type c = calend.GetType();

            PropertyInfo[] prop = calend.GetType().GetProperties();
            foreach (PropertyInfo p in prop)
            {
                Console.Write(p.Name + ": ");
                object[] attrs = p.GetCustomAttributes(false);
                foreach (object o in attrs)
                {
                    LuisIdentifierAttribute li = o as LuisIdentifierAttribute;
                    if (li != null)
                    {
                        Console.Write(li.Value + " ");
                    }
                    LuisBuiltInIdentifierAttribute lb = o as LuisBuiltInIdentifierAttribute;
                    if (lb != null)
                    {
                        Console.Write(lb.Value + " ");
                    }
                    IsRequiredAttribute ip = o as IsRequiredAttribute;
                    if (ip != null)
                    {
                        Console.Write(ip.Value + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
