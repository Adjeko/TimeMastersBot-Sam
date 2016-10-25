using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TimeMasters.Bot.Helpers.Luis;
using TimeMasters.Bot.Helpers.Luis.Calendar;
using Microsoft.Bot.Builder.Luis;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class UpdateDialog : LuisDialog<object>
    {
        private InformationManager<UpdateCalendar> calendarManager;
        private string _ask;

        public UpdateDialog(IDialogContext context, LuisResult lr)
        {
            _ask = "";
            calendarManager = new InformationManager<UpdateCalendar>();

            calendarManager.ProcessResult(lr);
        }

        public override async Task StartAsync(IDialogContext context)
        {
            ProcessManagerResult(context);
        }

        private void Say(IDialogContext context, string text)
        {
            var result = Task.Run(() => context.PostAsync(text));
        }

        [LuisIntent("UpdateCalendarEntry")]
        public async Task CreateEntry(IDialogContext context, LuisResult result)
        {
            calendarManager.ProcessResult(result);
            ProcessManagerResult(context);
        }

        [LuisIntent("AdditionalInformation")]
        public async Task AdditionalInformation(IDialogContext context, LuisResult result)
        {
            calendarManager.ProcessResult(result);
            ProcessManagerResult(context);
        }

        [LuisIntent("Test")]
        public async Task Test(IDialogContext context, LuisResult result)
        {
            foreach (UpdateCalendar c in calendarManager.Forms)
            {
                Say(context, c.ToString());
            }
            //context.Wait(MessageReceived);
        }

        private void ProcessManagerResult(IDialogContext context)
        {
            if (calendarManager.IsInformationRequired())
            {
                Say(context, calendarManager.GetNextMissingInformation());
                context.Wait(MessageReceived);
            }
            else
            {
                ConfirmWithUserPermission(context);
            }
        }

        public async void ConfirmWithUserPermission(IDialogContext context)
        {
            var list = calendarManager.GetFinishedEntries();
            foreach (var item in list)
            {
                _ask += $"{item}\n";
            }

            PromptDialog.Confirm(context,
                                ConfirmAsync,
                                $"Soll ich {_ask} für dich ändern ?",
                                "Das habe ich nicht verstanden.",
                                promptStyle: PromptStyle.None);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync($"Ich habe {_ask} für dich geändert");
            }
            else
            {
                await context.PostAsync("You just went full retard. Never go full retard.");
            }
            context.Done<string>("CREATE");
        }
    }
}