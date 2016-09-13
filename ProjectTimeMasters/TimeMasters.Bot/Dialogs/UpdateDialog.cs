using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace TimeMasters.Bot.Dialogs
{
    [Serializable]
    public class UpdateDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var temp = await argument;

            PromptDialog.Confirm(
                context,
                ConfirmAsync,
                "Du wolltest etwas updaten?",
                "Keine Ahnung was du vorhatest.",
                promptStyle: PromptStyle.None);

        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync("danke für das bestätigen");
            }
            else
            {
                await context.PostAsync("You just went full retard. Never go full retard.");
            }
            context.Done<IMessageActivity>(null);
        }
    }
}