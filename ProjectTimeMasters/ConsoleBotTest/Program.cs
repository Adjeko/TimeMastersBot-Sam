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

            //CLASS I Test Case
            Console.WriteLine("Test Case CLASS I:");

            List<IntentRecommendation> intentList = new List<IntentRecommendation>();
            intentList.Add(new IntentRecommendation() { Intent = "CreateCalendarEntry", Score = 0.9860273 });
            intentList.Add(new IntentRecommendation() { Intent = "UpdateCalendarEntry", Score = 1.09791529E-06 });
            intentList.Add(new IntentRecommendation() { Intent = "None", Score = 0.000106518892 });
            intentList.Add(new IntentRecommendation() { Intent = "DeleteCalendarEntry", Score = 0.0009615617 });

            List<EntityRecommendation> entityList = new List<EntityRecommendation>();
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "Calendar::StartDate", StartIndex = 17, EndIndex = 22, Score = 0.953924954 });
            entityList.Add(new EntityRecommendation() { Entity = "monday", Type = "Calendar::EndDate", StartIndex = 33, EndIndex = 38, Score = 0.9419334 });
            entityList.Add(new EntityRecommendation() { Entity = "camping", Type = "Calendar::Title", StartIndex = 4, EndIndex = 10, Score = 0.9891184 });
            entityList.Add(new EntityRecommendation() { Entity = "12:00", Type = "Calendar::StartTime", StartIndex = 24, EndIndex = 28, Score = 0.964997 });
            entityList.Add(new EntityRecommendation() { Entity = "18:00", Type = "Calendar::EndTime", StartIndex = 40, EndIndex = 44, Score = 0.9204767 });

            infoManager.ProcessResult(new LuisResult("add camping from friday 12:00 to monday 18:00", intentList, entityList));
            var result = infoManager.GetFinishedEntries();
            foreach (var c in result)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("\nPress Enter for Test Case CLASS II\n");
            Console.ReadLine();


            //CLASS II Test Case
            Console.WriteLine("Test Case CLASS II");
            intentList = new List<IntentRecommendation>();
            intentList.Add(new IntentRecommendation() { Intent = "CreateCalendarEntry", Score = 0.9908426 });
            intentList.Add(new IntentRecommendation() { Intent = "UpdateCalendarEntry", Score = 0.00581081724 });

            intentList.Add(new IntentRecommendation() { Intent = "None", Score = 0.0001621704 });
            intentList.Add(new IntentRecommendation() { Intent = "DeleteCalendarEntry", Score = 0.00120160938 });

            entityList = new List<EntityRecommendation>();
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "Calendar::StartDate", StartIndex = 15, EndIndex = 20, Score = 0.941222548 });
            entityList.Add(new EntityRecommendation() { Entity = "camping", Type = "Calendar::Title", StartIndex = 4, EndIndex = 10, Score = 0.995535135 });
            entityList.Add(new EntityRecommendation() { Entity = "17:00", Type = "Calendar::StartTime", StartIndex = 27, EndIndex = 31, Score = 0.8503845 });
            entityList.Add(new EntityRecommendation() { Entity = "19:00", Type = "Calendar::EndTime", StartIndex = 36, EndIndex = 40, Score = 0.916959941 });
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("date", "XXXX-WXX-5");
            entityList.Add(new EntityRecommendation(
) { Entity = "friday", Type = "builtin.datetime.date", StartIndex = 11, EndIndex = 16, Resolution = dic });

            infoManager.ProcessResult(new LuisResult("add camping on friday from 17:00 to 19:00", intentList, entityList));
            result = infoManager.GetFinishedEntries();
            foreach (var c in result)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("\nPress Enter for Test Case CLASS III\n");
            Console.ReadLine();

            //CLASS III Test Case
            Console.WriteLine("Test Case CLASS III");
            intentList = new List<IntentRecommendation>();
            intentList.Add(new IntentRecommendation() { Intent = "CreateCalendarEntry", Score = 0.9971466 });
            intentList.Add(new IntentRecommendation() { Intent = "UpdateCalendarEntry", Score = 0.018504601 });
            intentList.Add(new IntentRecommendation() { Intent = "None", Score = 0.000772698 });
            intentList.Add(new IntentRecommendation() { Intent = "DeleteCalendarEntry", Score = 0.0005007981 });

            entityList = new List<EntityRecommendation>();
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "Calendar::StartDate", StartIndex = 11, EndIndex = 16, Score = 0.9830753 });
            entityList.Add(new EntityRecommendation() { Entity = "sunday", Type = "Calendar::StartDate", StartIndex = 22, EndIndex = 27, Score = 0.9144727 });
            entityList.Add(new EntityRecommendation() { Entity = "gym", Type = "Calendar::Title", StartIndex = 4, EndIndex = 6, Score = 0.998410761 });
            entityList.Add(new EntityRecommendation() { Entity = "17:00", Type = "Calendar::StartTime", StartIndex = 32, EndIndex = 45, Score = 0.9840954 });
            entityList.Add(new EntityRecommendation() { Entity = "19:00", Type = "Calendar::EndTime", StartIndex = 41, EndIndex = 36, Score = 0.853437364 });
            dic = new Dictionary<string, string>();
            dic.Add("date", "XXXX-WXX-5");
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "builtin.datetime.date", StartIndex = 11, EndIndex = 16, Resolution = dic });

            infoManager.ProcessResult(new LuisResult("add gym on friday and sunday at 17:00 to 19:00", intentList, entityList));
            result = infoManager.GetFinishedEntries();
            foreach (var c in result)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("\nPress Enter for Test Case CLASS IV\n");
            Console.ReadLine();

            //CLASS IV Test Case
            Console.WriteLine("Test Case CLASS IV");
            intentList = new List<IntentRecommendation>();
            intentList.Add(new IntentRecommendation() { Intent = "CreateCalendarEntry", Score = 0.9998312 });
            intentList.Add(new IntentRecommendation() { Intent = "UpdateCalendarEntry", Score = 7.396022E-10 });
            intentList.Add(new IntentRecommendation() { Intent = "None", Score = 2.17381948E-05 });
            intentList.Add(new IntentRecommendation() { Intent = "DeleteCalendarEntry", Score = 0.0009163393 });

            entityList = new List<EntityRecommendation>();
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "Calendar::StartDate", StartIndex = 11, EndIndex = 16, Score = 0.992603958 });
            entityList.Add(new EntityRecommendation() { Entity = "gym", Type = "Calendar::Title", StartIndex = 4, EndIndex = 6, Score = 0.9993661 });
            entityList.Add(new EntityRecommendation() { Entity = "17:00", Type = "Calendar::StartTime", StartIndex = 21, EndIndex = 25, Score = 0.9925799 });


            infoManager.ProcessResult(new LuisResult("add gym on friday at 17:00", intentList, entityList));
            Console.WriteLine(infoManager.GetNextMissingInformation());

            //make the answer
            intentList.Add(new IntentRecommendation() { Intent = "CreateCalendarEntry", Score = 0.0306769088 });
            intentList.Add(new IntentRecommendation() { Intent = "UpdateCalendarEntry", Score = 0.00254854467 });
            intentList.Add(new IntentRecommendation() { Intent = "None", Score = 0.000767548452 });
            intentList.Add(new IntentRecommendation() { Intent = "DeleteCalendarEntry", Score = 0.0012494463 });
            intentList.Add(new IntentRecommendation() { Intent = "AdditionalInformation", Score = 0.9887786 });

            entityList = new List<EntityRecommendation>();
            entityList.Add(new EntityRecommendation() { Entity = "18:00", Type = "Calendar::EndTime", StartIndex = 21, EndIndex = 25, Score = 0.8685397 });
            entityList.Add(new EntityRecommendation() { Entity = "monday", Type = "Calendar::EndDate", StartIndex = 11, EndIndex = 16, Score = 0.9170408 });
            
            infoManager.ProcessResult(new LuisResult("it ends on monday at 18:00", intentList, entityList));
            result = infoManager.GetFinishedEntries();
            foreach (var c in result)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("\nPress Enter for Test Case CLASS V\n");
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
