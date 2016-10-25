using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TimeMasters.PortableClassLibrary.Logging;
using TimeMasters.Bot.Helpers.Luis;
using TimeMasters.Bot.Helpers.Luis.Calendar;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class DeleteDialog : LuisDialog<object>
    {
        private InformationManager<DeleteCalendar> calendarManager;
        private string _ask;

        public DeleteDialog(IDialogContext context, LuisResult lr)
        {
            _ask = "";
            calendarManager = new InformationManager<DeleteCalendar>();

            calendarManager.ProcessResult(lr);
        }

        private void Say(IDialogContext context, string text)
        {
            var result = Task.Run(() => context.PostAsync(text));
        }

        public override async Task StartAsync(IDialogContext context)
        {
            ProcessManagerResult(context);
        }


        [LuisIntent("DeleteCalendarEntry")]
        public async Task RemoveEntry(IDialogContext context, LuisResult result)
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
            foreach (DeleteCalendar c in calendarManager.Forms)
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

        private void ConfirmWithUserPermission(IDialogContext context)
        {
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
                                $"Soll ich {_ask} für dich löschen ?",
                                "Das habe ich nicht verstanden.",
                                promptStyle: PromptStyle.None);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync($"Ich habe {_ask} für dich gelöscht");
            }
            else
            {
                await context.PostAsync("You just went full retard. Never go full retard.");
            }
            context.Done<string>("REMOVE");
        }
    }
}