using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace TimeMasters.Bot.Dialogs
{
    [Serializable]
    public class ButtonDialog : IDialog<string>
    {
        string _outputText;
        string[] _buttonTexts;

        public ButtonDialog(string outputText, string[] buttonTexts)
        {
            _outputText = outputText;
            _buttonTexts = buttonTexts;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.AddHeroCard(_outputText, _buttonTexts);

            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }

        public async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> args)
        {
            var tmp = await args;
            context.Done(tmp.Text);
        }

        
    }
}