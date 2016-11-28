using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeMasters.Bot.Helpers.Luis.DateTimeParser
{
    public class DateTimeParser
    {
        private LuisResult _result;
        private string _debug;

        public DateTimeParser()
        {

        }

        public void SetLuisResult(LuisResult result)
        {
            _result = result;
        }

        public static DateTime ParseDateTime(LuisResult result, EntityRecommendation entity, string luisBuildtinIdentifier, out string debug)
        {
            string _debug = "";
            //find buildtin datetime 
            EntityRecommendation build = null;

            var findings = result.Entities.Where(e =>
            {
                return e.Type == luisBuildtinIdentifier && e.StartIndex == entity.StartIndex;
            });

            if (findings.Any())
            {
                build = findings.First();
            }


            if (build != null)
            {
                //parse buildtin
                IResolutionParser parse = new ResolutionParser();



                Resolution res;
                BuiltIn.DateTime.DateTimeResolution dtRes;

                parse.TryParse(build.Resolution, out res);
                dtRes = res as BuiltIn.DateTime.DateTimeResolution;
                //BuiltIn.DateTime.DateTimeResolution.TryParse(build.Resolution["date"], out dtRes);

                DateTime value = new DateTime(dtRes.Year.GetValueOrDefault(),
                    dtRes.Month.GetValueOrDefault(),
                    dtRes.Day.GetValueOrDefault(),
                    dtRes.Hour.GetValueOrDefault(),
                    dtRes.Minute.GetValueOrDefault(),
                    dtRes.Second.GetValueOrDefault());

                _debug += $"found buildtin type for {entity.Entity}\n\n";
                debug = _debug;
                return value;
            }
            else
            {
                _debug += $"No buildtin Entity found for {entity.Entity} with type {luisBuildtinIdentifier}\n\n";

                //clear all whitespaces
                string text = new string(entity.Entity.Where(c => !char.IsWhiteSpace(c)).ToArray());
                
                //try using Text to parse with Chronic
                var chronic = new Chronic.Parser().Parse(text);
                if(chronic != null)
                {
                    //Chronic success
                    
                }
            }

            //error
            _debug += $"{entity.Entity} with type {luisBuildtinIdentifier} could not be parsed\n\n";
            debug = _debug;
            return new DateTime();
        }

    }
}