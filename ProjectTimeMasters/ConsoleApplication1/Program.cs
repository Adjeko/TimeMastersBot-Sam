using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using TimeMastersClassLibrary.Database;
=======
using TimeMasters.PortableClassLibrary.DirectLine;
>>>>>>> master

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            DatabaseClient DBC = new DatabaseClient();
            DBC.Insert();
            Console.WriteLine("Läuft");
            Console.ReadLine();
=======

            string user = "Eduard";

            BotConnector connector = new BotConnector();
            connector.init();
            //Console.WriteLine(connector.httpTest());


            Console.WriteLine("Willkommen!");

            while (true)
            {
                Console.WriteLine(user + ": ");
                string text = Console.ReadLine();

                if (text == "finish")
                { 
                    Console.WriteLine("ENDE.");
                    break;
                }

                connector.sendMessage(user, text);
                List<MessageEntity> messages = connector.getMessage();

                Console.WriteLine("Bot: ");

                foreach (MessageEntity me in messages)
                {
                    Console.WriteLine(me.Message);
                }
            }
>>>>>>> master
        }
    }
}
