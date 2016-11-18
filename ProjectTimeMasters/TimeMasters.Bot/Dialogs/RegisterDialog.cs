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
    [LuisModel("e0547aea-007f-46ef-9134-00e0a5d3b6e1", "c53f28052cb343efac7ea618a3b666db")]
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