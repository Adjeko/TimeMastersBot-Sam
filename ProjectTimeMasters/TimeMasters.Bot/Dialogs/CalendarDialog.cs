using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using TimeMasters.Bot.Helpers.Luis;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using TimeMasters.PortableClassLibrary.Calendar.Google;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("e0547aea-007f-46ef-9134-00e0a5d3b6e1", "c53f28052cb343efac7ea618a3b666db")]
    [Serializable]
    public class CalendarDialog : LuisDialog<object>
    {

        protected dynamic calendarManager;
        protected string dialogName;
        protected string actionString; //temporary solution
        protected string _ask;
        protected dynamic list;

        public CalendarDialog(IDialogContext context, LuisResult result)
        {}

        protected void Say(IDialogContext context, string text)
        {
            var result = Task.Run(() => context.PostAsync(text));
        }

        public override async Task StartAsync(IDialogContext context)
        {
            ProcessManagerResult(context);
        }

        protected override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            //do some hard coded stuff
            IMessageActivity answer = item.GetAwaiter().GetResult();

            switch(answer.Text)
            {
                case "!debug":
                    Task.Run(() => DebugAsync(context));
                    break;
                case "!exit":
                    calendarManager.Clear();
                    context.Done($"Exited {dialogName}");
                    return Task.CompletedTask;
                    break;
                default:
                    //no command in Text. continue normal.
                    return base.MessageReceived(context, item);
                    break;
            };

            context.Wait(MessageReceived);
            return Task.CompletedTask;
        }

        [LuisIntent("Greetings")]
        public async Task GreetingsAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Greetings to you too :)");
            ProcessManagerResult(context);
        }

        [LuisIntent("None")]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"I didn't understand the intent of your message : {result.Query}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("DeleteCalendarEntry")]
        public async Task DeleteEntryAsync(IDialogContext context, LuisResult result)
        {
            calendarManager.ProcessResult(result);
            ProcessManagerResult(context);
        }

        [LuisIntent("UpdateCalendarEntry")]
        public async Task UpdateEntryAsync(IDialogContext context, LuisResult result)
        {
            calendarManager.ProcessResult(result);
            ProcessManagerResult(context);
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

        private async Task DebugAsync(IDialogContext context)
        {
            Say(context, calendarManager.GetDebugMessage());
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
            list = calendarManager.GetFinishedEntries();
            foreach (var item in list)
            {
                _ask += $"{item}\n\n";
            }

            context.Call(new ButtonDialog($"Soll ich {_ask} für dich {actionString}?", new string[] { "Yes", "No", "Change Entry" }), ConfirmAsync);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string confirm = await argument;
            switch (confirm)
            {
                case "Yes":
                    //TODO: create calendar entry in Google or Microsoft Calendar

                    if(dialogName == "Create")
                    {
                        GoogleCalendar google = new GoogleCalendar();
                        
                        foreach(var item in list)
                        {

                        }
                    }

                    await context.PostAsync($"Ich habe {_ask} für dich {actionString}.");
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

            context.Done(dialogName);
        }

    }
}