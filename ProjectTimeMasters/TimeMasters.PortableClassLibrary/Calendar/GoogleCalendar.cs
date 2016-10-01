using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMasters.PortableClassLibrary.Calendar
{
    class GoogleCalendar : ICalendar
    {
        public CalendarEntry CreateCalendarEntry(CalendarEntry entry)
        {
            Event newEvent = new Event()
            {
                Summary = entry.Name,
                Description = entry.Description,

                Start = new EventDateTime()
                {
                    DateTime = entry.StartTime,
                    TimeZone = "Europe/Zurich",
                },
                End = new EventDateTime()
                {
                    DateTime = entry.EndTime,
                    TimeZone = "Europe/Zurich"
                }
            };

            String calendarId = "primary";
            EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
            Event createdEvent = request.Execute();
            return CalendarEntry.ConvertFromGoogleEvent(createdEvent);
        }

        public CalendarEntry CreateCalendarEntry(string name, string description, DateTime startTime, DateTime endTime)
        {
            CalendarEntry newCalenderEntry = new CalendarEntry()
            {
                Name = name,
                Description = description,
                StartTime = startTime,
                EndTime = endTime

            };
            return CreateCalendarEntry(newCalenderEntry);
        }

        public bool DeleteCalendarEntry(string name, DateTime minTime, DateTime maxTime)
        {
            Event evt = GetEvent(name, minTime, maxTime);
            EventsResource.InsertRequest request = service.Events.delete("primary", evt.Id);
            request.Execute();
            return (true);
        }
        public List<CalendarEntry> GetCalendarEntries(DateTime startDate, DateTime endDate)
        {
            List<CalendarEntry> items = new List<CalendarEntry>();
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMax = endDate;
            request.TimeMin = startDate;
            Events events = request.Execute();
            foreach (Event e in events.Items)
            {
                items.Add(CalendarEntry.ConvertFromGoogleEvent(e));
            }
            return items;
        }

        public CalendarEntry GetCalendarEntry(string name, DateTime startTime, DateTime endTime)
        {
            
            return CalendarEntry.ConvertFromGoogleEvent(GetEvent(name, startTime, endTime));
        }

        private Event GetEvent(string name, DateTime startTime, DateTime endTime)
        {

            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMax = startTime;
            request.TimeMin = endTime;
            Events events = request.Execute();
            IEnumerable<Event> evt = events.Items.Where(E => E.Summary == name);

            return evt.ElementAt(0);
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
