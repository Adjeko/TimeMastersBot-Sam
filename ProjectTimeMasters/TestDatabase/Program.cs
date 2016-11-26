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
        static void Main()
        {
            GoogleCalenderTokens GCT = new GoogleCalenderTokens();
            string a = "abc1.5";
                
            string b = "abc2.1";
            string c = "abc3.1";
            long d = 100;
            string exampleTime = "03-06-2016 10:00 am";
            DateTime date = DateTime.Parse(exampleTime);

            string b2 = "";
            string c2 = "";
            long d2 = 0;
            DateTime date2 = new DateTime();

            //GCT.StoreCredential(a, b, c, d, date);
            //GCT.UpdateCredential(a, b, c, d, date);
            //GCT.GetCredential(a, out b2, out c2, out d2, out date2);
            GCT.DeleteCredential(a);
            Console.WriteLine($"{a} {b} {c} {d} {date}");
            Console.WriteLine($"{a} {b2} {c2} {d2} {date2}");
            Console.ReadLine();
        }
    }
}
