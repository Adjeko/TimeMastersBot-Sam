﻿using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMastersClassLibrary.Calendar
{
    public class CalendarEntry
    {
        private string _id;
        private string _name;
        private string _description;
        private DateTime _startTime;
        private DateTime _endTime;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public CalendarEntry()
        {

        }
        public static CalendarEntry ConvertFromGoogleEvent(Event gEvent)
        {
            CalendarEntry resultEntry = new CalendarEntry()
            {
                Name = gEvent.Summary,
                Description = gEvent.Description,
                StartTime = (DateTime)gEvent.Start.DateTime,
                EndTime = (DateTime)gEvent.End.DateTime,
                Id = gEvent.Id,
            };
            return resultEntry;
        }
    }
}
