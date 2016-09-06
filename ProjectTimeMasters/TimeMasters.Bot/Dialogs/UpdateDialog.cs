using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace TimeMasters.Bot.Dialogs
{
    public class UpdateDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            PromptDialog.Confirm(
                context,
                ConfirmAsync,
                "Du wolltest etwas updaten?",
                "Keine Ahnung was du vorhatest.",
                promptStyle: PromptStyle.None);
        }

        public async Task ConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            await context.PostAsync("Danke das du das bestätigt hast");
            context.Wait(MessageReceivedAsync);
        }
    }
}