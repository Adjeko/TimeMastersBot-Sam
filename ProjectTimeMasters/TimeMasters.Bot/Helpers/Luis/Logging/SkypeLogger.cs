using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TimeMasters.Bot.Helpers.Luis.Logging
{
    public class SkypeLogger
    {


        [Route("OutboundMessages/Skype")]
        [HttpPost]
        private async void WriteToSkype(string text)
        {

            using (var client = new ConnectorClient(new Uri("https://skype.botframework.com")))
            {
                var conversation = await client.Conversations.CreateDirectConversationAsync(new ChannelAccount(), new ChannelAccount("29:1QngyDg4BC9UlXB2xnyHPQecY8PQqlyQhqknZ4lqFE-g"));
                IMessageActivity message = Activity.CreateMessageActivity();
                message.From = new ChannelAccount();
                message.Recipient = new ChannelAccount("29:1QngyDg4BC9UlXB2xnyHPQecY8PQqlyQhqknZ4lqFE-g");
                message.Conversation = new ConversationAccount { Id = conversation.Id };
                message.Text = text;
                await client.Conversations.SendToConversationAsync((Activity)message);
            }

        }

    }
}