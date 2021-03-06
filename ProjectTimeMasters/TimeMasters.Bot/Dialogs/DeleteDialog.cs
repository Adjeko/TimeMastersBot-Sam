﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TimeMasters.Bot.Helpers.Luis;
using TimeMasters.Bot.Helpers.Luis.Calendar;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("e0547aea-007f-46ef-9134-00e0a5d3b6e1", "c53f28052cb343efac7ea618a3b666db")]
    [Serializable]
    public class DeleteDialog : CalendarDialog
    {
        public DeleteDialog(IDialogContext context, LuisResult lr, string userId, string userName) : base(context, lr, userId, userName)
        {
            dialogName = "Delete";
            actionString = "gelöscht";
            calendarManager = new InformationManager<DeleteCalendar>();

            calendarManager.ProcessResult(lr);
        }
        
    }
}