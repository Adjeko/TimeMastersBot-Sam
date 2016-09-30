using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMasters.PortableClassLibrary
{
    public class BotMessage
    {
        public string id { get; set; }
        public string conversationId { get; set; }
        public DateTime created { get; set; }
        public string from { get; set; }
        public string text { get; set; }
        public Channeldata channelData { get; set; }
        public string[] images { get; set; }
        public Attachment[] attachments { get; set; }
        public string eTag { get; set; }
    }

    public class Channeldata
    {
    }

    public class Attachment
    {
        public string url { get; set; }
        public string contentType { get; set; }
    }


    public class ConversationIdToken
    {
        public string conversationId { get; set; }
        public string token { get; set; }
    }


    public class Conversation
    {
        public BotMessage[] messages { get; set; }
        public string watermark { get; set; }
        public string eTag { get; set; }
    }
}
