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

        public static List<Tuple<int,LogMessage>> logs = new List<Tuple<int, LogMessage>>();

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a command");
            while(true)
            {
                string[] input = Console.ReadLine().Split(' ');
                string command = input[0];

                switch(command)
                {
                    case "ls":
                        List();
                        break;
                    case "search-name":
                        SearchName(input[1]);
                        break;
                    case "search-id":
                        SearchID(input[1]);
                        break;
                    case "see":
                        See(Int32.Parse(input[1]));
                        break;
                    case "load":
                        Load();
                        break;
                    case "quit":
                    case "exit":                        
                        return;
                    default:
                        Console.WriteLine("Command" + command + "not found ");
                        break;
                }
            }
        }

        public static void List()
        {
            foreach(var t in logs)
            {
                Console.WriteLine(" " + t.Item1 + ": " + t.Item2.message + " " + t.Item2.time + " ID:" + t.Item2.userId + " Name:" + t.Item2.userName);
            }
            if(logs.Count == 0)
            {
                Console.WriteLine("No Logs loaded");
            }
            else
            {
                Console.WriteLine("All Logs loaded");
            }
        }

        public static void SearchName(string input)
        {
            List<Tuple<int, LogMessage>> tmpLogs = new List<Tuple<int, LogMessage>>(logs);

            tmpLogs = tmpLogs.Where(e => e.Item2.userName == input).ToList();

            for (int i = 0; i < tmpLogs.Count; i++)
            {
                Console.WriteLine(" " + tmpLogs[i].Item1 + ": " + tmpLogs[i].Item2.message + " " + tmpLogs[i].Item2.time + " ID:" + tmpLogs[i].Item2.userId + " Name:" + tmpLogs[i].Item2.userName);
            }
            if(tmpLogs.Count == 0)
            {
                Console.WriteLine("No Entries found");
            }
        }

        public static void SearchID(string input)
        {
            List<Tuple<int, LogMessage>> tmpLogs = new List<Tuple<int, LogMessage>>(logs);

            tmpLogs = tmpLogs.Where(e => e.Item2.userId == input).ToList();

            for (int i = 0; i < tmpLogs.Count; i++)
            {
                Console.WriteLine(" " + tmpLogs[i].Item1 + ": " + tmpLogs[i].Item2.message + " " + tmpLogs[i].Item2.time + " ID:" + tmpLogs[i].Item2.userId + " Name:" + tmpLogs[i].Item2.userName);
            }
            if (tmpLogs.Count == 0)
            {
                Console.WriteLine("No Entries found");
            }
        }

        public static void See(int n)
        {
            var tmp = logs.Where(e => e.Item1 == n).ToList();
            if (tmp.Count == 0)
            {
                Console.WriteLine($"No Entry found for {n}");
                return;
            }
            LogMessage t = tmp.ElementAt(0).Item2;
            Console.WriteLine($"\n {t.level}   {t.time}\n{t.userId}   {t.userName}\n{t.message}\n{t.payload}\n\nEXCEPTION:\n{t.exceptionMessage}\n\n{t.stackTrace}\n\n");
        }

        public static void Load()
        {
            var client = new RestClient("http://timemastersbot.azurewebsites.net");
            var request = new RestRequest("/api/Logging", Method.GET);

            var tmp = client.Execute(request);
            var logStrings = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(tmp.Content);

            for(int i = logStrings.Count - 1; i >= 0; i--)
            {
                if(logStrings[i].Contains("Exception"))
                {
                    Console.WriteLine(logStrings[i]);
                    Console.WriteLine("unable to load logs");
                    return;
                }
                string s = logStrings[i];
                LogMessage t = LogMessage.GetFromJSONString(s);
                logs.Add(new Tuple<int, LogMessage>(i, t));
            }

            List();

            Console.WriteLine("Successfully loaded Logs");                         
        }
    }
}
