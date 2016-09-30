using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TimeMasters.Bot.Helpers.Luis
{
    public class InformationManager
    {
        public List<Information> Entities { get; set; }

        public InformationManager()
        {
            Entities = new List<Information>();
        }

        public void AddEntity(Information info)
        {
            Entities.Add(info);
        }

        public void ProcessResult(LuisResult result)
        {
            foreach(Information i in Entities)
            {
                i.SetEntitiy(SearchInLuisResult((dynamic)i, result));
            }
        }

        private object SearchInLuisResult<T>(T inf, LuisResult result)
        {
            //first search for normal recommendation
            //then search for matching build in recommendations
            EntityRecommendation rec;
            EntityRecommendation buildRec;
            if (typeof(T) == typeof(LuisInformation<string>))
            {
                var temp = inf as LuisInformation<string>;
                if (!result.TryFindEntity(temp.LuisIdentifier, out rec))
                    return null;

                return rec.Entity;

            }
            if (typeof(T) == typeof(LuisInformation<DateTime>))
            {
                var temp = inf as LuisInformation<DateTime>;
                DateTime datetime = new DateTime();

                if (!result.TryFindEntity(temp.LuisIdentifier, out rec))
                    return null;

                if (result.TryFindEntity(temp.LuisBuiltinIdentifier, out buildRec))
                {
                    var parser = new Chronic.Parser();
                    var chronic = parser.Parse(buildRec.Entity); //TODO parse buildRec.Resolution["date"]
                    datetime = chronic.ToTime();
                }

                return datetime;
            }
            if (typeof(T) == typeof(LuisInformation<LuisResult>))
            {
                return null;
            }
            throw new Exception("This LuisInformation Type is not supported");
        }

        public bool IsRequiredAvailable()
        {
            IEnumerable<Information> tmp = Entities.Where(info => (info.IsRequired && info.IsSet()));
            
            return tmp.Count() == Entities.Count(e => e.IsRequired);
        }

        public bool IsAllAvailable()
        {
            return Entities.Count(info => info.IsSet()) == Entities.Count;
        }

        public string GetMissingInformation()
        {
            if (IsRequiredAvailable())
            {
                return null;
            }

            string returnString = "I still need: \n";

            foreach (Information i in Entities)
            {
                if(i.IsRequired && !i.IsSet())
                    returnString += i.ToString() + "\n";
            }
            returnString += "Please provide the missing information";
            return returnString;
        }
    }
}