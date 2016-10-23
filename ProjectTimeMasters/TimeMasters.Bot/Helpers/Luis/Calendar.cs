using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TimeMasters.Bot.Helpers.Luis
{
    public class Calendar : ILuisForm
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
        [IsRequired(true)]
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

        public override string ToString()
        {
            return $"{Title} from {StartDate} + {StartTime} to {EndDate} + {EndTime}. Duration: {Duration}";
        }

        protected bool Equals(Calendar other)
        {
            return string.Equals(Title, other.Title)
                && StartTime.Equals(other.StartTime)
                && StartDate.Equals(other.StartDate)
                && OrigStartTime.Equals(other.OrigStartTime) 
                && OrigStartDate.Equals(other.OrigStartDate)
                && EndTime.Equals(other.EndTime)
                && EndDate.Equals(other.EndDate)
                && Duration.Equals(other.Duration);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Title?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ StartTime.GetHashCode();
                hashCode = (hashCode*397) ^ StartDate.GetHashCode();
                hashCode = (hashCode*397) ^ OrigStartTime.GetHashCode();
                hashCode = (hashCode*397) ^ OrigStartDate.GetHashCode();
                hashCode = (hashCode*397) ^ EndTime.GetHashCode();
                hashCode = (hashCode*397) ^ EndDate.GetHashCode();
                hashCode = (hashCode*397) ^ Duration.GetHashCode();
                return hashCode;
            }
        }

        public void TryResolveMissingInformation()
        {

            //if StartDate and StartTime are set, then assume that EndDate = StartDate
            if (!StartDate.Equals(new DateTime()) && !StartTime.Equals(new DateTime()) && !EndTime.Equals(new DateTime()) && EndDate.Equals(new DateTime()))
            {
                EndDate = StartDate;
            }
        }
    }
}