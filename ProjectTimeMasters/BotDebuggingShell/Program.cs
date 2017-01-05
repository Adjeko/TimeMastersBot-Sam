using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using TimeMastersClassLibrary.Logging;

namespace BotDebuggingShell
{
    class Program
    {

        public static List<string> logs;

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a command");
            while(true)
            {
                string[] input = Console.ReadLine().Split(' ');
                string command = input[0];

                switch(command)
                {
                    case "see":
                        See(Int32.Parse(input[1]));
                        break;
                    case "load":
                        Load();
                        break;
                    case "exit":                        
                        return;
                    default:
                        Console.WriteLine("Command" + command + "not found ");
                        break;
                }
            }
        }

        public static void See(int n)
        {
            Console.WriteLine(logs[n]);
        }

        public static void Load()
        {
            var client = new RestClient("http://timemastersbot.azurewebsites.net");
            var request = new RestRequest("/api/Logging", Method.GET);

            var tmp = client.Execute(request);
            logs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(tmp.Content);

            for(int i = 0; i < logs.Count; i++)
            {
                Console.WriteLine(i + ": " + logs[i]);
            }

            Console.WriteLine("Successfully loaded Logs");                         
        }
    }
}
