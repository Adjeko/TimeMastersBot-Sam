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

        private CalendarForm _referenceForm;
        public List<CalendarForm> Forms { get; set; }

        public InformationManager()
        {
            _referenceForm = new CalendarForm();
            Forms = new List<CalendarForm>();
        }

        public void AddEntity(Information info)
        {
            _referenceForm.Add(info);
        }

        public void ProcessResult(LuisResult result)
        {
            foreach(Information i in _referenceForm.Entries)
            {
                SearchInLuisResult((dynamic)i, result);
            }
        }

        private void SearchInLuisResult<T>(T inf, LuisResult result)
        {
            //first search for normal recommendation
            //then search for matching build in recommendations
            EntityRecommendation rec;
            EntityRecommendation buildRec;
            if (typeof(T) == typeof(LuisInformation<string>))
            {
                var input = inf as LuisInformation<string>;
                //if (!result.TryFindEntity(input.LuisIdentifier, out rec))
                //    return null;

                //return rec.Entity;

                var list = result.Entities.Where(e => e.Type == input.LuisIdentifier);
                
                if (input.LuisIdentifier == "Calendar::Title")
                {
                    CalendarForm tmp = _referenceForm;
                    
                }

            }
            if (typeof(T) == typeof(LuisInformation<DateTime>))
            {
                var temp = inf as LuisInformation<DateTime>;
                DateTime datetime = new DateTime();

                if (!result.TryFindEntity(temp.LuisIdentifier, out rec))
                    return;

                if (result.TryFindEntity(temp.LuisBuiltinIdentifier, out buildRec))
                {
                    var parser = new Chronic.Parser();
                    var chronic = parser.Parse(buildRec.Entity); //TODO parse buildRec.Resolution["date"]
                    datetime = chronic.ToTime();
                }

                return;
            }
            if (typeof(T) == typeof(LuisInformation<LuisResult>))
            {
                return;
            }
            throw new Exception("This LuisInformation Type is not supported");
        }

        public bool IsOneRequiredAvailable()
        {
            IEnumerable<CalendarForm> tmp = Forms.Where(form => form.IsRequiredAvailable());
            
            return tmp.Any();
        }

        public bool IsOneAllAvailable()
        {
            IEnumerable<CalendarForm> tmp = Forms.Where(form => form.IsAllAvailable());

            return tmp.Any();
        }

        public string GetMissingInformation()
        {
            return "";
        }
    }
}