using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalendarQuickstart
{
    class CalendarInsert
    {
        public void insert()
        {
            Event newEvent = new Event()
            {
                Summary = "Test Eintrag",
                Location = "Test Straße",
                Description = "Testen",

                Start = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2016-09-21"),
                    TimeZone = "Germany",

                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2016-09-22"),
                    TimeZone = "Germany",
                },
                Recurrence = new String[] { "RRULE:FREQ=WEEKLY;INTERVAL=2;COUNT=8;WKST=SU;BYDAY=TU,TH" },
                Attendees = new EventAttendee[]
                {
                new EventAttendee() {Email ="adjeko88@gmail.com" },
                },
                Reminders = new Event.RemindersData()
                {
                    UseDefault = false,
                    Overrides = new EventReminder[]
                    {
                    new EventReminder() { Method = "email", Minutes = 24 * 60 },
                    }
                }

            };
            String calendarId = "primary";
            EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
            Event createdEvent = request.Execute();
            Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
        }
    }
}
