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
using TimeMasters.PortableClassLibrary.Logging;

namespace TimeMasters.Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                //int length = (activity.Text ?? string.Empty).Length;

                ////return our reply to the user
                //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //await connector.Conversations.ReplyToActivityAsync(reply);
                activity.Text = Translator.Translate(activity.Text, Languages.English);
                Logger log = Logger.GetInstance();

                string exMessage = "";
                Activity logReply = activity.CreateReply("Try TestRestSharp");
                try
                {
                    logReply = activity.CreateReply("Try TestRestSharp");
                    await connector.Conversations.ReplyToActivityAsync(logReply);
                    log.SetClient();
                    logReply = activity.CreateReply("Client Success TestRestSharp");
                    await connector.Conversations.ReplyToActivityAsync(logReply);
                    log.SetRequest();
                    logReply = activity.CreateReply("Request Success TestRestSharp");
                    await connector.Conversations.ReplyToActivityAsync(logReply);
                    exMessage = log.ExecuteRequest();
                    logReply = activity.CreateReply($"{exMessage} Success TestRestSharp");
                    await connector.Conversations.ReplyToActivityAsync(logReply);
                }
                catch (System.Exception ex)
                {
                    logReply = activity.CreateReply($"Exception TestRestSharp => {ex.Message}");
                    await connector.Conversations.ReplyToActivityAsync(logReply);
                    exMessage += ex.Message;
                }


                Activity reply = activity.CreateReply($"Log: {log} is null ? {log == null} Trace: Message: {exMessage}");//Trace: {log.IsTraceEnabled} Debug: {log.IsDebugEnabled} Warn: {log.IsWarnEnabled} Error: {log.IsErrorEnabled} Fatal: {log.IsFatalEnabled} Info: {log.IsInfoEnabled}");
                await connector.Conversations.ReplyToActivityAsync(reply);


                await Conversation.SendAsync(activity, () => new TestDialog());

                
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