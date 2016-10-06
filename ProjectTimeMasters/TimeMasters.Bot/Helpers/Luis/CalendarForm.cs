using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeMasters.Bot.Helpers.Luis
{
    public class CalendarForm
    {
        public List<Information> Entries { get; set; }

        public CalendarForm()
        {
            Entries = new List<Information>();
        }

        public void Add(Information i)
        {
            Entries.Add(i);
        }

        public bool IsRequiredAvailable()
        {
            IEnumerable<Information> tmp = Entries.Where(info => info.IsRequired && info.IsSet());

            return tmp.Count() == Entries.Count(e => e.IsRequired);
        }

        public bool IsAllAvailable()
        {
            return Entries.Count(info => info.IsSet()) == Entries.Count;
        }
    }
}