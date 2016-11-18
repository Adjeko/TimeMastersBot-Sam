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
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class CreateDialog : LuisDialog<object>
    {
        private InformationManager<CreateCalendar> calendarManager;
        private string _ask;

        public CreateDialog(IDialogContext context, LuisResult lr)
        {
            _ask = "";
            calendarManager = new InformationManager<CreateCalendar>();

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

        [LuisIntent("None")]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I didn't understand");
            context.Wait(MessageReceived);
        }

        [LuisIntent("CreateCalendarEntry")]
        public async Task CreateEntryAsync(IDialogContext context, LuisResult result)
        {
            calendarManager.ProcessResult(result);
            ProcessManagerResult(context);
        }
        
        [LuisIntent("AdditionalInformation")]
        public async Task AdditionalInformationAsync(IDialogContext context, LuisResult result)
        {
            calendarManager.ProcessResult(result);
            ProcessManagerResult(context);
        }

        [LuisIntent("Test")]
        public async Task Test(IDialogContext context, LuisResult result)
        {
            Say(context, calendarManager.GetDebugMessage());
            //foreach (CreateCalendar c in calendarManager.Forms)
            //{
            //    Say(context, c.ToString());
            //}
            if(result.Query.Contains("exit"))
            {
                await context.PostAsync("leaving CreateDialog\n\n");
            }
            context.Wait(MessageReceived);
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
                ConfirmWithUserPermissionAsync(context);
            }
        }
        
        public async void ConfirmWithUserPermissionAsync(IDialogContext context)
        {
            var list = calendarManager.GetFinishedEntries();
            foreach (var item in list)
            {
                _ask += $"{item}\n\n";
            }

            context.Call(new ButtonDialog($"Soll ich {_ask} für dich erstellen?", new string[] {"Yes", "No", "Change Entry" }), ConfirmAsync);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string confirm = await argument;
            switch (confirm)
            {
                case "Yes":
                    //TODO: create calendar entry in Google or Microsoft Calendar
                    await context.PostAsync($"Ich habe {_ask} für dich erstellt.");
                    break;
                case "No":
                    await context.PostAsync("Dann verwerfe ich diese Informationen wieder.");
                    break;
                case "Change Entry":
                    //TODO:
                    await context.PostAsync("Ich würde dir damit gerne helfen, aber das kann ich noch nicht :(");
                    break;
                default:
                    await context.PostAsync("You just went full retard. Never go full retard.");
                    break;
            };

            context.Done<string>("CREATE");
        }
    }
}