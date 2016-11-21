using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TimeMasters.PortableClassLibrary;


namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("e0547aea-007f-46ef-9134-00e0a5d3b6e1", "c53f28052cb343efac7ea618a3b666db")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
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

            switch(answer.Text)
            {
                case "!register":
                    context.Call(new RegisterDialog(_userId, _userName), Done);
                    break;
                default:
                    return base.MessageReceived(context, item);
            };

            return Task.CompletedTask;
        }

        [LuisIntent("Greetings")]
        public async Task GreetingsAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hello {_userName}! I'm your Schedule & Appointment Manager but I'm also a Super Adorable Mate. You can just call me Sam :).");
            context.Wait(MessageReceived);
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand you (Id:{_userId} Name: {_userName})";
            await context.PostAsync(message);
            context.Wait(MessageReceived);

        }

        [LuisIntent("Test")]
        public async Task Test(IDialogContext context, LuisResult result)
        {
            context.Call(new ButtonDialog("TestFrage",new string[] {"Yes", "no", "maybe"}), TestDone);
        }

        public async Task TestDone(IDialogContext context, IAwaitable<string> args)
        {
            string tmp = await args;
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
            context.Call(new CreateDialog(context, result), Done);
        }

        [LuisIntent("DeleteCalendarEntry")]
        public async Task RemoveEntry(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("luis remove");
            context.Call(new DeleteDialog(context, result), Done);
        }

        [LuisIntent("UpdateCalendarEntry")]
        public async Task UpdateEntry(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("luis update");
            context.Call(new UpdateDialog(context, result), Done);
        }

        [LuisIntent("AdditionalInformation")]
        public async Task AdditionalInformationAsync(IDialogContext context, LuisResult result)
        {
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