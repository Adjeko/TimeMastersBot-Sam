using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using TimeMasters.PortableClassLibrary.Helpers;

namespace TimeMasters.PortableClassLibrary.Translator
{
    public static class Translator
    {
        private static AdmAccessToken admToken;
        private static string headerValue;

        private static AdmAuthentication admAuth = new AdmAuthentication("f13a223c-8b0d-4767-bc98-83d684df1f30", "SEEP/Wc16RNERuF0TUblJ/KdK49nXj1wm5yu/bP4aEo=");

        public static string Translate(string text, Languages toLanguage)
        {
            admToken = admAuth.GetAccessToken();
            headerValue = "Bearer " + admToken.access_token;
            
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" +
                         System.Web.HttpUtility.UrlEncode(text) + "&to=" + toLanguage.GetStringValue();
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
            httpWebRequest.Headers["Authorization"] = headerValue;
            WebResponse response = null;

            try
            {
                Task<WebResponse> responseTask = Task.Run(() => httpWebRequest.GetResponseAsync());
                response = responseTask.Result;
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                    string translation = (string) dcs.ReadObject(stream);
                    return translation;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return "Something bad happened during translation";
        }

        public static string Translate(string text, Languages fromLanguage, Languages toLanguage)
        {
            return "";
        }
    }
}