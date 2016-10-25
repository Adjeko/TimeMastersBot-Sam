using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeMasters.Bot.Helpers.Luis.Calendar
{
    [Serializable]
    public class UpdateCalendar : ILuisForm
    {
        [LuisIdentifier("Calendar::Title")]
        [IsRequired(true)]
        [IsPrimary(true)]
        public string Title { get; set; }

        [LuisIdentifier("Calendar::StartTime")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime StartTime
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(tmp.Year, tmp.Month, tmp.Day, StartDateTime.Hour, StartDateTime.Minute, StartDateTime.Second);
            }
            set
            {
                StartDateTime = new DateTime(StartDateTime.Year, StartDateTime.Month, StartDateTime.Day, value.Hour, value.Minute, value.Second);
            }
        }

        [LuisIdentifier("Calendar::StartDate")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [IsRequired(true)]
        [IsPrimary(false)]
        public DateTime StartDate
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(StartDateTime.Year, StartDateTime.Month, StartDateTime.Day, tmp.Hour, tmp.Minute, tmp.Second);
            }
            set
            {
                StartDateTime = new DateTime(value.Year, value.Month, value.Day, StartDateTime.Hour, StartDateTime.Minute, StartDateTime.Second);
            }
        }

        public DateTime StartDateTime;

        [LuisIdentifier("Calendar::OrigStartTime")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime OrigStartTime
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(tmp.Year, tmp.Month, tmp.Day, OrigStartDateTime.Hour, OrigStartDateTime.Minute, OrigStartDateTime.Second);
            }
            set
            {
                OrigStartDateTime = new DateTime(OrigStartDateTime.Year, OrigStartDateTime.Month, OrigStartDateTime.Day, value.Hour, value.Minute, value.Second);
            }
        }

        [LuisIdentifier("Calendar::OrigStartDate")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime OrigStartDate
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(OrigStartDateTime.Year, OrigStartDateTime.Month, OrigStartDateTime.Day, tmp.Hour, tmp.Minute, tmp.Second);
            }
            set
            {
                StartDateTime = new DateTime(value.Year, value.Month, value.Day, OrigStartDateTime.Hour, OrigStartDateTime.Minute, OrigStartDateTime.Second);
            }
        }

        public DateTime OrigStartDateTime;



        [LuisIdentifier("Calendar::EndTime")]
        [LuisBuiltInIdentifier("builtin.datetime.time")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime EndTime
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(tmp.Year, tmp.Month, tmp.Day, EndDateTime.Hour, EndDateTime.Minute, EndDateTime.Second);
            }
            set
            {
                EndDateTime = new DateTime(EndDateTime.Year, EndDateTime.Month, EndDateTime.Day, value.Hour, value.Minute, value.Second);
            }
        }

        [LuisIdentifier("Calendar::EndDate")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime EndDate
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(EndDateTime.Year, EndDateTime.Month, EndDateTime.Day, tmp.Hour, tmp.Minute, tmp.Second);
            }
            set
            {
                EndDateTime = new DateTime(value.Year, value.Month, value.Day, EndDateTime.Hour, EndDateTime.Minute, EndDateTime.Second);
            }
        }

        public DateTime EndDateTime;


        [LuisIdentifier("Calendar::MoveLaterTime")]
        [LuisBuiltInIdentifier("builtin.datetime.duration")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime MoveLaterTime { get; set; }

        [LuisIdentifier("Calendar::MoveForwardTime")]
        [LuisBuiltInIdentifier("builtin.datetime.duration")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime MoveForwardTime { get; set; }

        [LuisIdentifier("Calendar::Duration")]
        [LuisBuiltInIdentifier("builtin.datetime.duration")]
        [IsRequired(false)]
        [IsPrimary(false)]
        public DateTime Duration { get; set; }

        public UpdateCalendar()
        {

        }

        public override string ToString()
        {
            return $"{Title} from {StartDateTime} to {EndDateTime}. Duration: {Duration}";
        }

        protected bool Equals(CreateCalendar other)
        {
            return string.Equals(Title, other.Title)
                && StartTime.Equals(other.StartTime)
                && StartDate.Equals(other.StartDate)
                && StartDateTime.Equals(other.StartDateTime)
                && EndTime.Equals(other.EndTime)
                && EndDate.Equals(other.EndDate)
                && EndDateTime.Equals(other.EndDateTime)
                && Duration.Equals(other.Duration);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Title?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ StartTime.GetHashCode();
                hashCode = (hashCode * 397) ^ StartDate.GetHashCode();
                hashCode = (hashCode * 397) ^ EndTime.GetHashCode();
                hashCode = (hashCode * 397) ^ EndDate.GetHashCode();
                hashCode = (hashCode * 397) ^ Duration.GetHashCode();
                return hashCode;
            }
        }

        public void TryResolveMissingInformation()
        {
            throw new NotImplementedException();
        }
    }
}