using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using TimeMasters.PortableClassLibrary.Calendar.Google;
using Microsoft.Bot.Connector;
using TimeMastersClassLibrary.Database;

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
            //await context.PostAsync("Du willst den Google Calendar registrieren.\n " +
            //                        "Ich werde dir als nächstes den Link zur Bestätigungsseite von Google schicken.\n" +
            //                        "Nachdem du zugesagt hast, wirst du auf unsere Website weitergeleitet.\n" +
            //                        "Danach hast du dich erfolgreich registriert.\n\n" +
            //                        $"UserID: {_userId}");
            //await context.PostAsync(_google.GetAuthenticationRedirectUri(_userId));
            IMessageActivity message = context.MakeMessage();

            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = _google.GetAuthenticationRedirectUri(_userId),
                Type = "signin",
                Title = "Connect"
            };
            cardButtons.Add(plButton);

            SigninCard plCard = new SigninCard("Authorize me for your Google Calendar", cardButtons);

            if(message.Attachments == null)
            {
                message.Attachments = new List<Attachment>();
            }
            message.Attachments.Add(plCard.ToAttachment());

            await context.PostAsync(message);

            //wait for the user to finish the sign in process
            while (!GoogleTokkenHandler.UserCodeDictionary.ContainsKey(_userId))
            {}

            await context.PostAsync($"Danke ! {GoogleTokkenHandler.UserCodeDictionary[_userId]}");

            string accessToken, refreshToken;
            DateTime issued;
            long expires;

            await context.PostAsync("attempting getting tokens");
            if(!_google.GetAuthorizationTokens(GoogleTokkenHandler.UserCodeDictionary[_userId], out accessToken, out refreshToken, out issued, out expires))
            {
                await context.PostAsync("lost tokens");
                context.Done("Registration fucked up");
            }
            await context.PostAsync($"got tokens ACCESS: {accessToken} REFRESH: {refreshToken}");
            GoogleCalenderTokens tokenDatabase = new GoogleCalenderTokens();
            tokenDatabase.StoreCredential(_userId, accessToken, refreshToken, expires, issued);

            await context.PostAsync("stored in database");

            string accessToken2, refreshToken2;
            DateTime issued2;
            long expires2;

            tokenDatabase.GetCredential(_userId, out accessToken2, out refreshToken2, out expires2, out issued2);

            await context.PostAsync("read from database");

            await context.PostAsync($"{accessToken} -> {accessToken2}\n\n {refreshToken} -> {refreshToken2}\n\n{expires.Equals(expires2)}\n\n{issued.Equals(issued2)}\n\n");

            context.Done("Registration OK");
        }
    }
}