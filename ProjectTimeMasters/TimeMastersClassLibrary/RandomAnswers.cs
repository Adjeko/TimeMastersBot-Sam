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
            List<string> testList1 = new List<string> { "Rejection @name @id Statz 1", "Refasfkhaslfd @name @id Satz 2", "Hallo @name @id Sath 3" };
            List<string> testList2 = new List<string> { "Deine mUdda Sentence 1", "hhhhhhh Sentence 2", "Tshuschac Sentence 3" };

            fromFile.Add("rejection", testList1);
            fromFile.Add("egal", testList2);

            string ausgabe = Newtonsoft.Json.JsonConvert.SerializeObject(fromFile);

            File.AppendAllText("jsonTest.txt", ausgabe);
        }

        public static string GetSentence(string key, Dictionary<string, string> bigdic)
        {
            string fileString = File.ReadAllText("jsonTest.txt");

            Dictionary<string, List<string>> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(fileString);

            Random rnd = new Random();

             string result = dic[key][rnd.Next(0, dic[key].Count)];

            foreach (var x in bigdic)
            {
                if (result.Contains(x.Key))
                result = result.Replace(x.Key,x.Value);
            }


            return result;
        }

    }
}
