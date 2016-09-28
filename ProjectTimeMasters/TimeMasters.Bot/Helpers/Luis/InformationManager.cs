using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;

namespace TimeMasters.Bot.Helpers.Luis
{
    public class InformationManager
    {
        public List<Information> Entities { get; set; }

        public void AddEntity(Information info)
        {
            Entities.Add(info);
        }

        public void ProcessResult(LuisResult result)
        {
            
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
            return "Shut up!";
        }
    }
}