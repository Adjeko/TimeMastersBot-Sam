using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using TimeMasters.PortableClassLibrary.Calendar.Google;

namespace TimeMasters.Bot.Dialogs
{
    [LuisModel("8ee9bc34-b3fa-4029-a6d5-08b50b22aa18", "3b397c65c2114c759f2bf67c6d473df2")]
    [Serializable]
    public class RegisterDialog : LuisDialog<object>
    {
        private GoogleTokkenHandler _google;
        private string _userId;
        private string _userName;

        public RegisterDialog(string id, string name)
        {
            _google = new GoogleTokkenHandler();
            _userId = id;
            _userName = name;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Du willst den Google Calendar registrieren.\n " +
                                    "Ich werde dir als nächstes den Link zur Bestätigungsseite von Google schicken.\n" +
                                    "Nachdem du zugesagt hast, wirst du auf unsere Website weitergeleitet.\n" +
                                    "Danach hast du dich erfolgreich registriert.\n\n" +
                                    $"UserID: {_userId}");
            await context.PostAsync(_google.GetAuthenticationRedirectUri(_userId));

            while (!GoogleTokkenHandler.UserCodeDictionary.ContainsKey(_userId))
            {}

            await context.PostAsync($"Danke ! {GoogleTokkenHandler.UserCodeDictionary[_userId]}");
            //_google.GetAuthorizationTokens(GoogleTokkenHandler.UserCodeDictionary[_userId]);
        }
    }
}