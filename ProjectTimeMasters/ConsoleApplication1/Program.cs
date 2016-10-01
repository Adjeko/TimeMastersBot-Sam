using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeMastersClassLibrary.Database;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseClient DBC = new DatabaseClient();
            DBC.Insert();
            Console.WriteLine("Läuft");
            Console.ReadLine();
        }
    }
}
