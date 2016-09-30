using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class CalendarDialog : LuisDialog<object>
    {
        [NonSerialized]
        protected LuisResult _LuisResult;

        public CalendarDialog(IDialogContext context, LuisResult result)
        {
            
        }
    }
}