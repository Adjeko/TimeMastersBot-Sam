using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using TimeMasters.Bot.Helpers.Luis;
using TimeMasters.Bot.Helpers.Luis.Calendar;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("e0547aea-007f-46ef-9134-00e0a5d3b6e1", "c53f28052cb343efac7ea618a3b666db")]
    [Serializable]
    public class CreateDialog : CalendarDialog
    {
        public CreateDialog(IDialogContext context, LuisResult lr) : base(context, lr)
        {
            dialogName = "Create";
            actionString = "erstellt";
            calendarManager = new InformationManager<CreateCalendar>();

            calendarManager.ProcessResult(lr);
        }
    }
}