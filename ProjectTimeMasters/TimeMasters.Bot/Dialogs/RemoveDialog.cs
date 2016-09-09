using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using TimeMasters.PortableClassLibrary.Logging;

namespace TimeMasters.Bot.Dialogs
{
    public class RemoveDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("RemoveDialog StartAsync");
            context.Wait(MessageReceivedAsync);

        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            await context.PostAsync("RemoveDialog StartAsync");
            try
            {
                PromptDialog.Confirm(
                    context,
                    ConfirmAsync,
                    "Du wolltest etwas löschen?",
                    "Keine Ahnung was du vorhatest.",
                    promptStyle: PromptStyle.None);
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().Error<RemoveDialog>("WAS IS DO LOS?", ex);
            }
            await context.PostAsync("RemoveDialog StartAsync fertig");
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