using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using TimeMasters.PortableClassLibrary.Logging;

namespace TimeMasters.Bot.Dialogs
{
    [Serializable]
    public class RemoveDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("RemoveDialog StartAsync");
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            await context.PostAsync($"RemoveDialog MessageReceivedAsync {message.Text}");
            PromptDialog.Confirm(
                    context,
                    ConfirmAsync,
                    "Du wolltest etwas löschen?",
                    "Keine Ahnung was du vorhatest.",
                    promptStyle: PromptStyle.None);
            
            await context.PostAsync("RemoveDialog StartAsync fertig");
            //context.Wait(MessageReceivedAsync);
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