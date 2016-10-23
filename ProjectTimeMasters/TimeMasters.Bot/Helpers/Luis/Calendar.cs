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
        public DateTime StartTime
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(tmp.Year, tmp.Month, tmp.Day, _startDateTime.Hour, _startDateTime.Minute, _startDateTime.Second);
            }
            set
            {
                _startDateTime = new DateTime(_startDateTime.Year, _startDateTime.Month, _startDateTime.Day, value.Hour, value.Minute, value.Second);
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
                return new DateTime(_startDateTime.Year, _startDateTime.Month, _startDateTime.Day, tmp.Hour, tmp.Minute, tmp.Second);
            }
            set
            {
                _startDateTime = new DateTime(value.Year, value.Month, value.Day, _startDateTime.Hour, _startDateTime.Minute, _startDateTime.Second);
            }
        }

        private DateTime _startDateTime;

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
        public DateTime EndTime
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(tmp.Year, tmp.Month, tmp.Day, _endDateTime.Hour, _endDateTime.Minute, _endDateTime.Second);
            }
            set
            {
                _endDateTime = new DateTime(_endDateTime.Year, _endDateTime.Month, _endDateTime.Day, value.Hour, value.Minute, value.Second);
            }
        }

        [LuisIdentifier("Calendar::EndDate")]
        [LuisBuiltInIdentifier("builtin.datetime.date")]
        [IsRequired(true)]
        [IsPrimary(false)]
        public DateTime EndDate
        {
            get
            {
                DateTime tmp = new DateTime();
                return new DateTime(_endDateTime.Year, _endDateTime.Month, _endDateTime.Day, tmp.Hour, tmp.Minute, tmp.Second);
            }
            set
            {
                _endDateTime = new DateTime(value.Year, value.Month, value.Day, _endDateTime.Hour, _endDateTime.Minute, _endDateTime.Second);
            }
        }

        private DateTime _endDateTime;

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
            return $"{Title} from {_startDateTime} to {_endDateTime}. Duration: {Duration}";
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