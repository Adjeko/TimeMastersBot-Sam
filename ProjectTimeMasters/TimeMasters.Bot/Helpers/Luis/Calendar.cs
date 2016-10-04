using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeMasters.Bot.Helpers.Luis
{
    public class Calendar
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime OrigStart { get; set; }
        //TODO define MoveLaterTime and MoveForwardTime
        public DateTime End { get; set; }
        public DateTime Duration { get; set; }

        public Calendar()
        {
            
        }

        public void SetStartDate(DateTime date)
        {
            Start = new DateTime(date.Year, date.Month, date.Day, Start.Hour, Start.Minute, Start.Second);
        }

        public void SetStartTime(DateTime time)
        {
            Start = new DateTime(Start.Year, Start.Month, Start.Day, time.Hour, time.Minute, time.Second);
        }

        public void SetOrigStartDate(DateTime date)
        {
            OrigStart = new DateTime(date.Year, date.Month, date.Day, OrigStart.Hour, OrigStart.Minute, OrigStart.Second);
        }

        public void SetOrigStartTime(DateTime time)
        {
            OrigStart = new DateTime(OrigStart.Year, OrigStart.Month, OrigStart.Day, time.Hour, time.Minute, time.Second);
        }

        public void SetEndDate(DateTime date)
        {
            End = new DateTime(date.Year, date.Month, date.Day, End.Hour, End.Minute, End.Second);
        }

        public void SetEndTime(DateTime time)
        {
            End = new DateTime(End.Year, End.Month, End.Day, time.Hour, time.Minute, time.Second);
        }

        public void SetDurationDate(DateTime date)
        {
            Duration = new DateTime(date.Year, date.Month, date.Day, Duration.Hour, Duration.Minute, Duration.Second);
        }

        public void SetDurationTime(DateTime time)
        {
            Duration = new DateTime(Duration.Year, Duration.Month, Duration.Day, time.Hour, time.Minute, time.Second);
        }

    }
}