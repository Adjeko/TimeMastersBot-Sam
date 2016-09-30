using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMasters.PortableClassLibrary.DirectLine
{
    class BotConnector
    {
    }

    static void httpTest()
    {
        // create conversation
        RestClient client = new RestClient("https://directline.botframework.com");

        var request = new RestRequest("/api/conversations", Method.POST);
        request.AddHeader("Authorization",
            "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

        var tmp = Task.Run(() => client.Execute(request));
        tmp.Wait();
        IRestResponse response = tmp.Result;
        Console.WriteLine(response.Content);

        ConversationIdToken tmpToken = JsonConvert.DeserializeObject<ConversationIdToken>(response.Content);


        // send message



        Console.ReadLine();
    }


    public class CoolMessage
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
}
