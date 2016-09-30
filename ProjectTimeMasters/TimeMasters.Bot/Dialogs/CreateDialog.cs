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

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class CreateDialog : LuisDialog<object>
    {
        [NonSerialized]
        private LuisResult _luisResult;

        private string _createEntity;
        private DateTime _inputDateTime;

        public CreateDialog(IDialogContext context, LuisResult lr)
        {
            _luisResult = lr;
            EntityRecommendation createEntry;
            if (lr.TryFindEntity("Calendar::Title", out createEntry))
            {
                _createEntity = createEntry.Entity;
            }
            DateTime Date = new DateTime();
            DateTime Time = new DateTime();
            if (lr.TryFindEntity("Calendar::StartDate", out createEntry))
            {
                EntityRecommendation date;
                if (lr.TryFindEntity("builtin.datetime.date", out date))
                {
                    Say(context, "Recommendation: " + date.Entity + "\n" + date.Resolution["date"]);
                    var parser = new Chronic.Parser();
                    var datetime = parser.Parse(date.Resolution["date"]);
                    //Date = datetime.ToTime();
                }
                if (lr.TryFindEntity("builtin.datetime.date", out date))
                {
                    Say(context, "Recommendation: " + date.Entity + "\n" + date.Resolution["date"]);
                    var parser = new Chronic.Parser();
                    var datetime = parser.Parse(date.Resolution["date"]);
                    //Date = datetime.ToTime();
                }
            }
            if (lr.TryFindEntity("Calendar::StartTime", out createEntry))
            {
                EntityRecommendation date;
                if (lr.TryFindEntity("builtin.datetime.time", out date))
                {
                    var parser = new Chronic.Parser();
                    var datetime = parser.Parse(date.Entity);
                    Time = datetime.ToTime();
                }
            }
            _inputDateTime = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second);
        }

        public override async Task StartAsync(IDialogContext context)
        {
            if (_luisResult.Entities.Count == 0)
            {
                GatherMissingInformation(context);
                context.Wait(MessageReceived);
            }
            else
            {
                CreateEntry(context);
            }
        }

        private void Say(IDialogContext context, string text)
        {
            var result = Task.Run(() => context.PostAsync(text));
        }

        [LuisIntent("CreateCalendarEntry")]
        public async Task CreateEntry(IDialogContext context, LuisResult result)
        {
            EntityRecommendation createEntry;
            if (result.TryFindEntity("Calendar::Title", out createEntry))
            {
                _createEntity = createEntry.Entity;
            }
            await context.PostAsync($"Title: {_createEntity}");
            DateTime Date = new DateTime();
            DateTime Time = new DateTime();
            if (result.TryFindEntity("Calendar::StartDate", out createEntry))
            {
                EntityRecommendation date;
                if (result.TryFindEntity("builtin.datetime.date", out date))
                {
                    var parser = new Chronic.Parser();
                    var datetime = parser.Parse(date.Entity);
                    Date = datetime.ToTime();
                }
            }
            await context.PostAsync($"Date: {Date}");
            if (result.TryFindEntity("Calendar::StartTime", out createEntry))
            {
                EntityRecommendation date;
                if (result.TryFindEntity("builtin.datetime.Time", out date))
                {
                    var parser = new Chronic.Parser();
                    var datetime = parser.Parse(date.Entity);
                    Time = datetime.ToTime();
                }
            }
            await context.PostAsync($"Time: {Time}");
            CreateEntry(context);
        }

        public async void GatherMissingInformation(IDialogContext context)
        {
            await context.PostAsync("Mir fehlen noch Informationen!");
            await context.PostAsync("Was möchtest du erstellen?");
        }

        public async void CreateEntry(IDialogContext context)
        {
            //TODO: create calendar entry

            PromptDialog.Confirm(context,
                                ConfirmAsync,
                                $"Soll ich {_createEntity} um {_inputDateTime} für dich erstellen ?",
                                "Das habe ich nicht verstanden.",
                                promptStyle: PromptStyle.None);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync($"Ich habe {_createEntity} um {_inputDateTime} für dich erstellt");
            }
            else
            {
                await context.PostAsync("You just went full retard. Never go full retard.");
            }
            context.Done<string>("CREATE");
        }
    }
}