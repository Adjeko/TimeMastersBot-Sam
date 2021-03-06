﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeMasters.Bot.Helpers.Luis.Calendar
{
    [Serializable]
    public class DeleteCalendar : ILuisForm
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
        [IsRequired(false)]
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

        public DeleteCalendar()
        {

        }

        public DateTime EndDateTime;

        public override string ToString()
        {
            return $"{Title} from {StartDateTime} to {EndDateTime}. ";
        }

        protected bool Equals(CreateCalendar other)
        {
            return string.Equals(Title, other.Title)
                && StartTime.Equals(other.StartTime)
                && StartDate.Equals(other.StartDate)
                && StartDateTime.Equals(other.StartDateTime)
                && EndTime.Equals(other.EndTime)
                && EndDate.Equals(other.EndDate)
                && EndDateTime.Equals(other.EndDateTime);
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
                return hashCode;
            }
        }

        public void TryResolveMissingInformation()
        {
            //if times and startdate are defined the enddate is probably is the same as the startdate
            if (!StartDate.Equals(new DateTime())
                && !StartTime.Equals(new DateTime())
                && !EndTime.Equals(new DateTime())
                && EndDate.Equals(new DateTime()))
            {
                EndDate = StartDate;
            }

            //if Start and End are defined the user probably wants to delete everything in this range
            if (Title == null &&
               !(StartDateTime == new DateTime()) &&
               !(EndDateTime == new DateTime()))
            {
                Title = "Everything";
            }
            
            return;
        }
    }
}