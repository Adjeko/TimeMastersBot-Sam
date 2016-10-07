using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TimeMasters.Bot.Helpers.Luis
{
    public class Calendar
    {
        [LuisIdentifier("Calendar::Title")]
        [IsRequired(true)]
        [IsPrimary(true)]
        public string Title { get; set; }

        [LuisIdentifier("Calendar::StartDate")]
        [LuisIdentifier("Calendar::StartTime")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(true)]
        [IsPrimary(false)]
        public DateTime Start { get; set; }

        [LuisIdentifier("Calendar::OrigStartDate")]
        [LuisIdentifier("Calendar::OrigStartTime")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(false)]
        [IsPrimary(true)]
        public DateTime OrigStart { get; set; }

        //TODO define MoveLaterTime and MoveForwardTime

        [LuisIdentifier("Calendar::EndDate")]
        [LuisIdentifier("Calendar::EndTime")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(true)]
        [IsPrimary(false)]
        public DateTime End { get; set; }

        [LuisIdentifier("Calendar::Duration")]
        [LuisBuiltInIdentifier("builtin.datetime.duration")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime Duration { get; set; }

        public Calendar()
        {
            
        }
    }
}