using System;
using System.Collections.Generic;
using System.Linq;
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

        public CreateDialog(LuisResult lr)
        {
            _luisResult = lr;
            EntityRecommendation updateEntry;
            if (lr.TryFindEntity("Calendar::Title", out updateEntry))
            {
                _createEntity = updateEntry.Entity;
            }
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

        [LuisIntent("CreateCalendarEntry")]
        public async Task CreateEntry(IDialogContext context, LuisResult result)
        {
            EntityRecommendation updateEntry;
            if (result.TryFindEntity("Calendar::Title", out updateEntry))
            {
                _createEntity = updateEntry.Entity;
            }
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
                                $"Soll ich {_createEntity} für dich erstellen ?",
                                "Das habe ich nicht verstanden.",
                                promptStyle: PromptStyle.None);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync($"Ich habe {_createEntity} für dich erstellt");
            }
            else
            {
                await context.PostAsync("You just went full retard. Never go full retard.");
            }
            context.Done<string>("CREATE");
        }
    }
}