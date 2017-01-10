using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using TimeMasters.Bot.Dialogs;
using TimeMasters.PortableClassLibrary.Helpers;
using TimeMasters.PortableClassLibrary.Translator;
using TimeMasters.Bot.Helpers.Luis.Logging;
using TimeMastersClassLibrary.Logging;

namespace TimeMasters.Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {


#pragma warning disable IDE1006 // Benennungsstile
                               /// <summary>
                               /// POST: api/Messages
                               /// Receive a message from a user and reply to it
                               /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
#pragma warning restore IDE1006 // Benennungsstile
        {
            if (activity.Type == ActivityTypes.Message)
            {
                try
                { 
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    // calculate something for us to return
                    //int length = (activity.Text ?? string.Empty).Length;

                    ////return our reply to the user
                    //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                    //await connector.Conversations.ReplyToActivityAsync(reply);

                    //activity.Text = Translator.Translate(activity.Text, Languages.English);

                    if(activity.ChannelId == "skype")
                    {
                        if (activity.Conversation.IsGroup.GetValueOrDefault())
                        {
                            activity.Text = activity.Text.Remove(0, activity.Text.LastIndexOf('>') + 2);
                        }
                    }
                    
                    await Conversation.SendAsync(activity, () => new RootDialog(activity.From.Name, activity.From.Id));
                    LoggerFactory.GetFileLogger().Info<MessagesController>(activity.From.Id, activity.From.Name, $"User said: {activity.Text}");
                }
                catch (System.Exception ex)
                {
                    LoggerFactory.GetFileLogger().Fatal<MessagesController>(activity.From.Id, activity.From.Name, "A Fatal Error occurred", ex);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}