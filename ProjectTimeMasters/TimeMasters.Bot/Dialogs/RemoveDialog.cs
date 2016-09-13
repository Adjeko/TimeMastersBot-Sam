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

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class RemoveDialog : LuisDialog<object>
    {
        [NonSerialized]
        private LuisResult _luisResult;

        private string _updateEntity;

        public RemoveDialog(LuisResult lr)
        {
            _luisResult = lr;
            _updateEntity = lr.Entities[0].Entity;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            //await base.StartAsync(context);

            if (_luisResult.Entities.Count == 0)
            {
                await context.PostAsync("Mir fehlen noch Informationen!");
            }
            else
            {
                PromptDialog.Confirm(context,
                                    ConfirmAsync,
                                    $"Soll ich {_luisResult.Entities[0].Entity} für dich löschen ?",
                                    "Das habe ich nicht verstanden.",
                                    promptStyle: PromptStyle.None);
            }

            //context.Wait(MessageReceived);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync($"Ich habe {_updateEntity} für dich gelöscht");
            }
            else
            {
                await context.PostAsync("You just went full retard. Never go full retard.");
            }
            context.Done<string>("Remove ist fertig jetzt amk");
        }
    }
}