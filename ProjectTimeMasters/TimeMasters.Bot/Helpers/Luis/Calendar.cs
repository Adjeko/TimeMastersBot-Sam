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

        [LuisIdentifier("Calendar::StartTime")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(true)]
        [IsPrimary(false)]
        public DateTime StartTime { get; set; }

        [LuisIdentifier("Calendar::StartDate")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [IsRequired(true)]
        [IsPrimary(false)]
        public DateTime StartDate { get; set; }

        [LuisIdentifier("Calendar::OrigStartTime")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime OrigStartTime { get; set; }

        [LuisIdentifier("Calendar::OrigStartDate")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime OrigStartDate { get; set; }

        //TODO define MoveLaterTime and MoveForwardTime

        [LuisIdentifier("Calendar::EndTime")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(true)]
        [IsPrimary(false)]
        public DateTime EndTime { get; set; }

        [LuisIdentifier("Calendar::EndDate")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime EndDate { get; set; }

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