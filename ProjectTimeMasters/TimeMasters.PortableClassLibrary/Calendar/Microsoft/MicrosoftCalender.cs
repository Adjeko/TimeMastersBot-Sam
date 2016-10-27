using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Core;
using Microsoft.Office365.OutlookServices;

namespace TimeMasters.PortableClassLibrary.Calendar.Microsoft
{
    class MicrosoftCalender : ICalendar
    {
        public CalendarEntry CreateCalendarEntry(CalendarEntry entry)
        {
            throw new NotImplementedException();
        }

        public CalendarEntry CreateCalendarEntry(string name, string description, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();

        }

        public bool DeleteCalendarEntry(string name, DateTime minTime, DateTime maxTime)
        {
            throw new NotImplementedException();
        }

        public List<CalendarEntry> GetCalendarEntries(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public CalendarEntry GetCalendarEntry(string name, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public CalendarEntry UpdateCalendarEntry(string name, DateTime startDate, DateTime endDate, CalendarEntry entry)
        {
            throw new NotImplementedException();
        }

        public CalendarEntry UpdateCalendarEntry(string oldName, DateTime startDate, DateTime endDate, string newName, string description, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
