using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;


namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class TestDialog : LuisDialog<object>
    {

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("addCalendarEntry")]
        public async Task AddEntry(IDialogContext context, LuisResult result)
        {
            string message = $"I've added ";

            EntityRecommendation activityName;
            if (!result.TryFindEntity("ActivityName", out activityName))
            {
                message += "NO NAME ";
            }
            else
            {
                message += activityName.Entity + " ";
            }

            message += "on ";

            EntityRecommendation datetime;
            if (!result.TryFindEntity("builtin.datetime.date", out datetime))
            {
                message += "NO DATETIME ";
            }
            else
            {
                var parser = new Chronic.Parser();
                var date = parser.Parse(datetime.Entity);
                message += date.ToString() + " ";
            }

            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }

        [LuisIntent("removeCalendarEntry")]
        public async Task RemoveEntry(IDialogContext context, LuisResult result)
        {
            PromptDialog.Confirm(
                context,
                ConfirmRemoveAsync,
                "Du wolltest etwas löschen?",
                "Keine Ahnung was du vorhatest.",
                promptStyle: PromptStyle.None);
        }

        [LuisIntent("updateCalendarEntry")]
        public async Task UpdateEntry(IDialogContext context, LuisResult result)
        {
            PromptDialog.Confirm(
                context,
                ConfirmUpdateAsync,
                "Du wolltest etwas updaten?",
                "Keine Ahnung was du vorhatest.",
                promptStyle: PromptStyle.None);
        }

        public async Task ConfirmRemoveAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            await context.PostAsync("Danke das du das bestätigt hast");
            //context.Wait<bool>(TestDialog.RemoveEntry);
        }

        public async Task ConfirmUpdateAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            await context.PostAsync("Danke das du das bestätigt hast");
            //context.Wait(UpdateEntry);
        }
    }
}