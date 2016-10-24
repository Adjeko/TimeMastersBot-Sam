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
using Calendar = TimeMasters.Bot.Helpers.Luis.Calendar;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class CreateDialog : LuisDialog<object>
    {
        private InformationManager<Calendar> calendarManager;
        private string _ask;

        public CreateDialog(IDialogContext context, LuisResult lr)
        {
            _ask = "";
            calendarManager = new InformationManager<Calendar>();

            calendarManager.ProcessResult(lr);
            if (calendarManager.IsInformationRequired())
            {
                Say(context, calendarManager.GetNextMissingInformation());
            }
        }

        public override async Task StartAsync(IDialogContext context)
        {
            if (calendarManager.IsInformationRequired())
            {
                context.Wait(MessageReceived);
            }
            else
            {
                AskForUserPermission(context);
            }
        }

        private void Say(IDialogContext context, string text)
        {
            var result = Task.Run(() => context.PostAsync(text));
        }

        [LuisIntent("CreateCalendarEntry")]
        public async Task CreateEntry(IDialogContext context, LuisResult result)
        {
            calendarManager.ProcessResult(result);
            if (calendarManager.IsInformationRequired())
            {
                Say(context, calendarManager.GetNextMissingInformation());
                context.Wait(MessageReceived);
            }
            else
            {
                AskForUserPermission(context);
            }
        }
        
        [LuisIntent("AdditionalInformation")]
        public async Task AdditionalInformation(IDialogContext context, LuisResult result)
        {
            calendarManager.ProcessResult(result);
            if (calendarManager.IsInformationRequired())
            {
                Say(context, calendarManager.GetNextMissingInformation());
                context.Wait(MessageReceived);
            }
            else
            {
                AskForUserPermission(context);
            }
        }

        [LuisIntent("Test")]
        public async Task Test(IDialogContext context, LuisResult result)
        {
            foreach (Calendar c in calendarManager.Forms)
            {
                Say(context, c.ToString());
            }
            //context.Wait(MessageReceived);
        }
        
        public async void AskForUserPermission(IDialogContext context)
        {
            //TODO: create calendar entry in Google or Microsoft Calendar

            var list = calendarManager.GetFinishedEntries();
            string ask = "Soll ich ";
            foreach (var item in list)
            {
                _ask += $"{item}\n";
            }
            ask += _ask;
            ask += " für dich erstellen ?";

            PromptDialog.Confirm(context,
                                ConfirmAsync,
                                ask,
                                "Das habe ich nicht verstanden.",
                                promptStyle: PromptStyle.None);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync($"Ich habe {_ask} für dich erstellt");
            }
            else
            {
                await context.PostAsync("You just went full retard. Never go full retard.");
            }
            context.Done<string>("CREATE");
        }
    }
}