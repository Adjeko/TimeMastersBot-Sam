using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using Newtonsoft.Json;

namespace TimeMasters.PortableClassLibrary.DirectLine
{
    public class BotConnector
    {
        public BotConnector()
        {

        }

        public void httpTest()
        {
            // create conversation
            RestClient client = new RestClient("https://directline.botframework.com");

            var request = new RestRequest("/api/conversations", Method.POST);
            request.AddHeader("Authorization",
                "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

            var tmp = Task.Run(() => client.Execute(request));
            tmp.Wait();
            IRestResponse response = tmp.Result;

            ConversationIdToken tmpToken = JsonConvert.DeserializeObject<ConversationIdToken>(response.Content);


            // send message

            request = new RestRequest("/api/conversations/" + tmpToken.conversationId + "/messages", Method.POST);

            //BotMessage message = new BotMessage()
            //{
            //    conversationId = tmpToken.conversationId,
            //    created = DateTime.Now,
            //    from = "Eduard",
            //    text = "Hallo du mudda",

            //};

            //request.AddJsonBody(message);

            //var tmp_2 = Task.Run(() => client.Execute(request));
            //tmp.Wait();
            //IRestResponse response_2 = tmp_2.Result;


        }

    }
}
