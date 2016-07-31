using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using TimeMasters.PortableClassLibrary.Helpers;
using Xamarin.Forms;

namespace TimeMasters.PortableClassLibrary.Translator
{
    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string _clientId;
        private string _clientSecret;
        private readonly string _request;
        private AdmAccessToken _token;
        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private Timer _accessTokenTimer;
        private const int RefreshTokenDuration = 9;
        public AdmAuthentication(string clientId, string clientSecret)
        {
            this._clientId = clientId;
            this._clientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            this._request =
                $"grant_type=client_credentials&client_id={HttpUtility.UrlEncode(clientId)}&client_secret={HttpUtility.UrlEncode(clientSecret)}&scope=http://api.microsofttranslator.com";
            Task<AdmAccessToken> tokenTask = Task.Run(() => this.HttpPost(DatamarketAccessUri, this._request));
            this._token = tokenTask.Result;
            //renew the token every specified minutes
            _accessTokenTimer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }
        public AdmAccessToken GetAccessToken()
        {
            return this._token;
        }
        private async void RenewAccessToken()
        {
            AdmAccessToken newAccessToken = await HttpPost(DatamarketAccessUri, this._request);
            //swap the new token with old one
            //Note: the swap is thread unsafe
            this._token = newAccessToken;
            //Console.WriteLine(string.Format("Renewed token for user: {0} is: {1}", this.clientId, this.token.access_token));
        }
        private void OnTokenExpiredCallback(object state)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                _accessTokenTimer.Dispose();
                _accessTokenTimer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
            }
        }
        private async Task<AdmAccessToken> HttpPost(string DatamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(DatamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            
            byte[] bytes = Encoding.UTF8.GetBytes(requestDetails);
            //webRequest.ContentLength = bytes.Length;
            using (Stream outputStream = await webRequest.GetRequestStreamAsync())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = await webRequest.GetResponseAsync())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }
}