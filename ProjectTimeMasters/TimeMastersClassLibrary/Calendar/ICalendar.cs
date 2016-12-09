using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMastersClassLibrary.Calendar
{
    public interface ICalendar
    {
        /// <summary>
        /// Creates a CalendarEntry in the respective calendar with the specified parameters.
        /// </summary>
        /// <param name="name">name of the CalendarEntry and displayname.</param>
        /// <param name="description">Text that describes the CalendarEntry.</param>
        /// <param name="startTime">Beginning of the Event described by the CalendarEntry.</param>
        /// <param name="endTime">Finish of the Event described by the CalendarEntry.</param>
        /// <returns>The newly created CalendarEntry. NULL if the creation failed.</returns>
        CalendarEntry CreateCalendarEntry(string name, string description, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Creates a CalendarEntry in the respective calendar with the specified parameters.
        /// </summary>
        /// <param name="entry">A complex object which describes the CalendarEntry to create.</param>
        /// <returns>The newly created CalendarEntry. NULL if the creation failed.</returns>
        CalendarEntry CreateCalendarEntry(CalendarEntry entry);


        /// <summary>
        /// Deletes a CalendarEntry in the respective calendar
        /// </summary>
        /// <param name="name">name of the CalendarEntry to delete.</param>
        /// <param name="minTime">start Date (inclusive) of the range in which the CalendarEntry has to be found.</param>
        /// <param name="maxTime">end Time (exclusive) of the range in which the CalendarEntry has to be found.</param>
        /// <returns>True if the deletion succeded. False otherwise.</returns>
        bool DeleteCalendarEntry(string name, DateTime minTime, DateTime maxTime);

        /// <summary>
        /// Updates an already existing CalendarEntry with a new Entry respecting the specified parameters.
        /// </summary>
        /// <param name="oldName">Name of the old CalendarEntry and displayname.</param>
        /// <param name="startDate">start Date (inclusive) of the range in which the CalendarEntry has to be found.</param>
        /// <param name="endDate">end Time (exclusive) of the range in which the CalendarEntry has to be found.</param>
        /// <param name="newName">name of the CalendarEntry and displayname.</param>
        /// <param name="description">Text that describes the new CalendarEntry.</param>
        /// <param name="startTime">Beginning of the Event described by the CalendarEntry.</param>
        /// <param name="endTime">Finish of the Event described by the CalendarEntry.</param>
        /// <returns>The newly updated CalendarEntry. NULL if the creation failed.</returns>
        CalendarEntry UpdateCalendarEntry(string oldName, DateTime startDate, DateTime endDate, string newName, string description, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Updates an already existing CalendarEntry with a new Entry respecting the specified parameters.
        /// </summary>
        /// <param name="name">Name of the old CalendarEntry and displayname.</param>
        /// <param name="startDate">start Date (inclusive) of the range in which the CalendarEntry has to be found.</param>
        /// <param name="endDate">end Time (exclusive) of the range in which the CalendarEntry has to be found.</param>
        /// <param name="entry">A complex object which describes the CalendarEntry to replace the old one with</param>
        /// <returns>The newly updated CalendarEntry. NULL if the creation failed.</returns>
        CalendarEntry UpdateCalendarEntry(string name, DateTime startDate, DateTime endDate, CalendarEntry entry);


        /// <summary>
        /// Returns the CalendarEntry found by the specified parameters.
        /// </summary>
        /// <returns>The found CalendarEntry. NULL if no CalendarEntry was found.</returns>
        CalendarEntry GetCalendarEntry(string name, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Returns a List of CalendarEntries found in the given timerange.
        /// </summary>
        /// <param name="startDate">start Date (inclusive) of the range in which the CalendarEntry has to be found.</param>
        /// <param name="endDate">end Time (exclusive) of the range in which the CalendarEntry has to be found.</param>
        /// <returns>List of all found CalendarEntries</returns>
        List<CalendarEntry> GetCalendarEntries(DateTime startDate, DateTime endDate);
    }
}
