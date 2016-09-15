using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;


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

        [LuisIntent("CreateCalendarEntry")]
        public async Task AddEntry(IDialogContext context, LuisResult result)
        {
            //string message = $"I've added ";
            //DateTime dateTime = new DateTime();
            //DateTime timeTime = new DateTime();
            //DateTime startTime;

            //EntityRecommendation recommendation;
            //if (!result.TryFindEntity("Calendar::Title", out recommendation))
            //{
            //    message += "NO NAME ";
            //}
            //else
            //{
            //    message += recommendation.Entity + " ";
            //}

            //message += "on ";

            //if (!result.TryFindEntity("Calendar::StartDate", out recommendation))
            //{
            //    message += "NO DATETIME ";
            //}
            //else
            //{
            //    EntityRecommendation date;
            //    if(!result.TryFindEntity("builtin.datetime.date", out date))
            //    {
            //        message += recommendation.Entity + " ";
            //    }
            //    else
            //    {
            //        var parser = new Chronic.Parser();
            //        var datetime = parser.Parse(date.Entity);
            //        dateTime = datetime.ToTime();
            //        //message += datetime.ToTime().ToString() + " ";
            //    }
            //}

            //message += "at ";

            //if (!result.TryFindEntity("Calendar::StartTime", out recommendation))
            //{
            //    message += "NO DATETIME ";
            //}
            //else
            //{
            //    EntityRecommendation time;
            //    if (!result.TryFindEntity("builtin.datetime.time", out time))
            //    {
            //        message += recommendation.Entity + " ";
            //    }
            //    else
            //    {
            //        var parser = new Chronic.Parser();
            //        var datetime = parser.Parse(time.Entity);
            //        timeTime = datetime.ToTime();
            //        //message += datetime.ToTime().ToString() + " ";
            //    }
            //}

            //startTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, timeTime.Hour, timeTime.Minute,
            //   timeTime.Second);
            //message += startTime.ToString();
           

            //await context.PostAsync(message);

            //context.Wait(MessageReceived);

            await context.PostAsync("luis create");
            context.Call(new CreateDialog(result), Done);
        }

        [LuisIntent("DeleteCalendarEntry")]
        public async Task RemoveEntry(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("luis remove");
            context.Call(new RemoveDialog(result), Done);
        }

        [LuisIntent("UpdateCalendarEntry")]
        public async Task UpdateEntry(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("luis update");
            context.Call(new UpdateDialog(result), Done);
        }

        public async Task Done(IDialogContext context, IAwaitable<object> input)
        {

            string temp = (await input) as string;
            await context.PostAsync($"Fertig mit {temp} ... AMK");
            context.Wait(MessageReceived);
        }
    }
}