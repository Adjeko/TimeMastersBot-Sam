using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TimeMasters.PortableClassLibrary;
using TimeMastersClassLibrary.Database;
using TimeMastersClassLibrary.Calendar.Google;
using TimeMastersClassLibrary.Logging;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("e0547aea-007f-46ef-9134-00e0a5d3b6e1", "c53f28052cb343efac7ea618a3b666db")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        private const string VERSION = "Sam v0.0.4J";


        private string _userId;
        private string _userName;

        public RootDialog(string id, string name)
        {
            _userId = id;
            _userName = name;
        }

        public override Task StartAsync(IDialogContext context)
        {
            return base.StartAsync(context);
        }

        protected override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            
            IMessageActivity answer = item.GetAwaiter().GetResult();
            LoggerFactory.GetFileLogger().Trace<RootDialog>(_userId, _userName, $"Root MessageReceived: {answer.Text}");
            switch (answer.Text)
            {
                case "!register":
                    context.Call(new RegisterDialog(_userId, _userName), Done);
                    break;
                case "!version":
                    context.PostAsync($"{VERSION} Name: {_userName} ID: {_userId}");
                    context.Wait(MessageReceived);
                    break;
                case "!refresh":
                    string accToken, refreshToken;
                    long lifetime;
                    DateTime createDate;

                    GoogleCalenderTokens google = new GoogleCalenderTokens();
                    google.GetCredential(_userId, out accToken, out refreshToken, out lifetime, out createDate);
                    context.PostAsync("got Credentials");

                    GoogleTokkenHandler tokens = new GoogleTokkenHandler();
                    if (tokens.RenewAccessToken(refreshToken, out accToken, out createDate, out lifetime))
                    {
                        context.PostAsync($"success to Refresh Tokens");
                        google.UpdateCredential(_userId, accToken, refreshToken, lifetime, createDate);
                        context.PostAsync("updated Credentials");
                    }
                    else
                    {
                        context.PostAsync($"{accToken}");
                    }

                    //TEST google service
                    GoogleCalendar calendar = new GoogleCalendar();
                    calendar.SetService(GoogleTokkenHandler.GetCalendarService(_userId));
                    calendar.CreateCalendarEntry("Bot Test Eintrag", $"hoffentlich klappts {_userId}", new DateTime(2016, 12, 10, 13, 0, 0), new DateTime(2016, 12, 10, 15, 0, 0));


                    context.Wait(MessageReceived);
                    break;
                case "!id":
                    context.PostAsync($"{_userId}");
                    context.Wait(MessageReceived);
                    break;
                default:
                    return base.MessageReceived(context, item);
            };

            return Task.CompletedTask;
        }

        [LuisIntent("Greetings")]
        public async Task GreetingsAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hello {_userName}! I'm your Schedule & Appointment Manager but I'm also a Super Adorable Mate. You can just call me S.A.M :).");
            context.Wait(MessageReceived);
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand you {result.Query} (Id:{_userId} Name: {_userName})";
            await context.PostAsync(message);
            context.Wait(MessageReceived);

        }

        [LuisIntent("Test")]
        public async Task Test(IDialogContext context, LuisResult result)
        {
            context.Wait(MessageReceived);
        }
        

        [LuisIntent("Register")]
        public async Task Register(IDialogContext context, LuisResult result )
        {
            await context.PostAsync("luis register");
            context.Call(new RegisterDialog(_userId, _userName), Done);
        }

        public async Task GoogleCode(IDialogContext context, IAwaitable<string> input)
        {
            /*string code = await input;
            await context.PostAsync("Habe Code: " + code + " erhalten");

            TestClassLibrary.TestGoogle tmp = new TestClassLibrary.TestGoogle();

            await context.PostAsync(tmp.TestGrant(code));

            await context.PostAsync("End Test Code Programm");
            context.Wait(MessageReceived);*/
        }

        [LuisIntent("CreateCalendarEntry")]
        public async Task AddEntry(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("luis create");
            context.Call(new CreateDialog(context, result, _userId, _userName), Done);
        }

        [LuisIntent("DeleteCalendarEntry")]
        public async Task RemoveEntry(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("luis remove");
            context.Call(new DeleteDialog(context, result, _userId, _userName), Done);
        }

        [LuisIntent("UpdateCalendarEntry")]
        public async Task UpdateEntry(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("luis update");
            context.Call(new UpdateDialog(context, result, _userId, _userName), Done);
        }

        [LuisIntent("AdditionalInformation")]
        public async Task AdditionalInformationAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("luis additional information");
            context.Wait(MessageReceived);
        }

        public async Task Done(IDialogContext context, IAwaitable<object> input)
        {
            string temp = (await input) as string;
            await context.PostAsync($"Fertig mit {temp}");
            context.Wait(MessageReceived);
        }
    }
}