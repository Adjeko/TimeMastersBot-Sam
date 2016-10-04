using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using TimeMasters.Bot.Helpers.Luis;

namespace ConsoleBotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LuisInformationTester();
            Console.Read();

        }

        public static void LuisInformationTester()
        {
            InformationManager infoManager = new InformationManager();
            infoManager.AddEntity(new LuisInformation<string> { IsRequired = true, LuisIdentifier = "Calendar::Title", LuisBuiltinIdentifier = "" });
            infoManager.AddEntity(new LuisInformation<DateTime> { IsRequired = false, LuisIdentifier = "Calendar::StartDate", LuisBuiltinIdentifier = "builtin.datetime.date" });
            infoManager.AddEntity(new LuisInformation<LuisResult>() { IsRequired = false, LuisIdentifier = "Shit::RandomNumber", LuisBuiltinIdentifier = "" });

            List<IntentRecommendation> intentList = new List<IntentRecommendation>();
            intentList.Add(new IntentRecommendation() { Intent = "CreateCalendarEntry", Score = 0.9971466 });
            intentList.Add(new IntentRecommendation() { Intent = "UpdateCalendarEntry", Score = 0.018504601 });
            intentList.Add(new IntentRecommendation() { Intent = "None", Score = 0.000772698 });
            intentList.Add(new IntentRecommendation() { Intent = "DeleteCalendarEntry", Score = 0.0005007981 });

            List<EntityRecommendation> entityList = new List<EntityRecommendation>();
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "Calendar::StartDate", StartIndex = 11, EndIndex = 16, Score = 0.9830753 });
            entityList.Add(new EntityRecommendation() { Entity = "gym", Type = "Calendar::Title", StartIndex = 4, EndIndex = 6, Score = 0.998410761 });
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("date", "XXXX-WXX-5");
            entityList.Add(new EntityRecommendation() { Entity = "friday", Type = "builtin.datetime.date", StartIndex = 11, EndIndex = 16, Resolution = dic });

            infoManager.ProcessResult(new LuisResult("add gym on friday", intentList, entityList));
            Console.WriteLine($"Required: {infoManager.IsRequiredAvailable()} + All: {infoManager.IsAllAvailable()}");
            Console.WriteLine(infoManager.GetMissingInformation());
            Console.Read();
        }
    }
}
