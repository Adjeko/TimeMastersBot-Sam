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
        private RestClient _client;
        private string _watermark = "0";
        private string _conversationId;

        public BotConnector()
        {
            _client = new RestClient("https://directline.botframework.com");
        }

        public string httpTest()
        {
            // create conversation
            RestClient client = new RestClient("https://directline.botframework.com");

            var request = new RestRequest("/api/conversations", Method.POST);
            request.AddHeader("Authorization",
                "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

            var tmp = Task.Run(() => client.Execute(request));
            tmp.Wait();

            ConversationIdToken tmpToken = JsonConvert.DeserializeObject<ConversationIdToken>(tmp.Result.Content);


            // send message

            request = new RestRequest("/api/conversations/" + tmpToken.conversationId + "/messages", Method.POST);
            request.AddHeader("Authorization",
                "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

            BotMessage message = new BotMessage()
            {
                conversationId = tmpToken.conversationId,
                created = DateTime.Now,
                from = "Eduard",
                text = "hallo",

            };

            request.AddJsonBody(message);

            var tmp_2 = Task.Run(() => client.Execute(request));
            tmp_2.Wait();

            //request = new RestRequest("/api/conversations/" + tmpToken.conversationId + "/messages", Method.POST);
            //request.AddHeader("Authorization",
            //    "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

            //BotMessage message_2 = new BotMessage()
            //{
            //    conversationId = tmpToken.conversationId,
            //    created = DateTime.Now,
            //    from = "Eduard",
            //    text = "hallo1",

            //};

            //request.AddJsonBody(message_2);

            //var tmp_4 = Task.Run(() => client.Execute(request));
            //tmp_4.Wait();
            //IRestResponse response_2 = tmp_2.Result;


            // get message
            request = new RestRequest("/api/conversations/" + tmpToken.conversationId + "/messages", Method.GET);
            request.AddHeader("Authorization",
                "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

            var tmp_3 = Task.Run(() => client.Execute(request));
            tmp_3.Wait();
            return tmp_3.Result.Content;

        }

        public void init()
        {
            var requestC = new RestRequest("/api/conversations", Method.POST);
            requestC.AddHeader("Authorization",
                "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

            var tmp = Task.Run(() => _client.Execute(requestC));
            tmp.Wait();

            ConversationIdToken tmpToken = JsonConvert.DeserializeObject<ConversationIdToken>(tmp.Result.Content);
            _conversationId = tmpToken.conversationId;
        }

        public void sendMessage(string author, string message)
        {

            var request = new RestRequest("/api/conversations/" + _conversationId + "/messages", Method.POST);
            request.AddHeader("Authorization",
                "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

            BotMessage BotMessage = new BotMessage()
            {
                conversationId = _conversationId,
                created = DateTime.Now,
                from = author,
                text = message,
            };

            request.AddJsonBody(BotMessage);

            var tmp = Task.Run(() => _client.Execute(request));
            tmp.Wait();
        }

        public List<MessageEntity> getMessage()
        {


            var request = new RestRequest("/api/conversations/" + _conversationId + "/messages", Method.GET);
            request.AddHeader("Authorization",
                "BotConnector 37LvAy_DTvg.cwA.MkE.EHi6Df92B1der8aplUkULsg_PHoFYNT1T0na4TWV3_s");

            request.AddParameter("watermark", _watermark);

            var tmp = Task.Run(() => _client.Execute(request));
            tmp.Wait();

            Conversation tmpToken = JsonConvert.DeserializeObject<Conversation>(tmp.Result.Content);

            _watermark = tmpToken.watermark;

            List<MessageEntity> list = new List<MessageEntity>();

            foreach(BotMessage m in tmpToken.messages)
            {
                if (m.from == "TimeMastersBot")
                {
                    MessageEntity me = new MessageEntity();
                    //me.Author = m.from;
                    me.Message = m.text;
                    list.Add(me);
                }
            }

            return list;
        }
    }
}
