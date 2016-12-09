using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSONTestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            GoogleAuthorizationCodeFlow flow;

            flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
            {
                ClientSecrets = new ClientSecrets()
                {
                    ClientId = "ClientId",
                    ClientSecret = "ClientSecret"
                },
                Scopes = new[] { CalendarService.Scope.Calendar },
                //DataStore = new FileDataStore("TimeMasters")
            });

            Console.WriteLine("Start");
            string flowString = Newtonsoft.Json.JsonConvert.SerializeObject(flow);//, new JsonSerializerSettings
            //{
            //    ContractResolver = SerializeContractResolver.Instance
            //});

            Console.WriteLine(flowString);

            Console.WriteLine("Deserialize");
            //GoogleAuthorizationCodeFlow newFlow = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleAuthorizationCodeFlow>(flowString);

            Console.WriteLine("Programm END");
            Console.ReadLine();
        }
    }

    public class SerializeContractResolver : DefaultContractResolver
    {
        public new static readonly SerializeContractResolver Instance = new SerializeContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (property.DeclaringType == typeof(GoogleAuthorizationCodeFlow) && property.PropertyName == "includeGrantedScopes")
            {
                property.Ignored = true;
            }
            return property;
        }
    }

}
