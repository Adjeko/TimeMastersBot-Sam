using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMastersClassLibrary
{
    public class RandomAnswers
    {
        private static Dictionary<string, List<string>> fromFile;

        public static void JSONToFile()
        {
            fromFile = new Dictionary<string, List<string>>();
            List<string> testList1 = new List<string> {"Rejection Statz 1", "Refasfkhaslfd Satz 2", "Hallo Sath 3" };
            List<string> testList2 = new List<string> { "Deine mUdda Sentence 1", "hhhhhhh Sentence 2", "Tshuschac Sentence 3" };

            fromFile.Add("rejection", testList1);
            fromFile.Add("egal", testList2);

            string ausgabe = Newtonsoft.Json.JsonConvert.SerializeObject(fromFile);

            File.AppendAllText("jsonTest.txt", ausgabe);
        }

        public static string FileToJSON()
        {
            string fileString = File.ReadAllText("jsonTest2.txt");

            Dictionary<string, List<string>> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(fileString);

            return dic["bitch"][0];
        }

    }
}
