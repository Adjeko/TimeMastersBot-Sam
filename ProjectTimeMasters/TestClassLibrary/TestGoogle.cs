using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using TimeMasters.PortableClassLibrary.Logging;
using Exception = System.Exception;

//using Google.Apis.Auth.OAuth2.Mvc;

namespace TestClassLibrary
{
    public class TestGoogle
    {

        public void TestCodeFlow(out string uri)
        {
            Task<AuthorizationCodeWebApp.AuthResult> resultTask = null;
            AuthorizationCodeWebApp.AuthResult result = null;
            try
            {
                IAuthorizationCodeFlow flow =
                    new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
                    {
                        ClientSecrets = new ClientSecrets()
                        {
                            ClientId = "505197371577-pu20ckhu9efr09kst8f5rmob4rssa9vr.apps.googleusercontent.com",
                            ClientSecret = "JrpjK2xRj2UDNRGGSM1lKjX7"
                        },
                        Scopes = new[] {CalendarService.Scope.Calendar},
                        DataStore = new FileDataStore("TimeMasters")
                    });


                resultTask =
                    new AuthorizationCodeWebApp(flow, "http://timemastersweb.azurewebsites.net/Home", "Bot")
                        .AuthorizeAsync
                        ("Adjeko88@gmail.com", CancellationToken.None);

                result = resultTask.Result;
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().Error<TestGoogle>("CodeFlow Exc", ex);
            }
            uri = result?.RedirectUri + "\n" + result.Credential?.UserId;
        }

        public string TestGrant(string code)
        {
            string resultString = "";
            try
            {
                var tokenRequest = new AuthorizationCodeTokenRequest();
                tokenRequest.Code = code;
                tokenRequest.GrantType = "authorization_code";
                tokenRequest.RedirectUri = "http://timemastersweb.azurewebsites.net/Home";
                tokenRequest.ClientId = "505197371577-pu20ckhu9efr09kst8f5rmob4rssa9vr.apps.googleusercontent.com";
                tokenRequest.ClientSecret = "JrpjK2xRj2UDNRGGSM1lKjX7";
                tokenRequest.Scope = CalendarService.Scope.Calendar;

                Task<TokenResponse> tokenResponseTask = tokenRequest.ExecuteAsync(new HttpClient(), "https://accounts.google.com/o/oauth2/token", CancellationToken.None,
                    SystemClock.Default);

                TokenResponse tokenResponse = tokenResponseTask.Result;
                tokenResponse.Issued = DateTime.Now;
                resultString = $"Access: {tokenResponse.AccessToken} Refresh: {tokenResponse.RefreshToken} LifeTime: {tokenResponse.ExpiresInSeconds} Issued: {tokenResponse.Issued} Scope: {tokenResponse.Scope}" ;
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().Error<TestGoogle>("TestGrant", ex);
            }
            return resultString;
        }

        public void TestWebBroker()
        {
            Task<UserCredential> credentialTask = null;

            try
            {
                credentialTask = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets()
                {
                    ClientId = "505197371577-pu20ckhu9efr09kst8f5rmob4rssa9vr.apps.googleusercontent.com",
                    ClientSecret = "JrpjK2xRj2UDNRGGSM1lKjX7"
                },
                new[] { CalendarService.Scope.Calendar },
                "Adjeko88@gmail.com",
                CancellationToken.None);

                UserCredential credential = credentialTask.Result;

                CalendarService service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "TimeMasters Bot"
                });
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().Error<TestGoogle>("Google fucked up", credentialTask.Exception.InnerException);
            }
        }

        public async Task<string> TestAuthorizationCodeFlow()
        {

            Logger logger = Logger.GetInstance();

            try
            {

                IAuthorizationCodeFlow googleAuthorizationCodeFlow =
                    new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
                    {
                        ClientSecrets = new ClientSecrets()
                        {
                            ClientId = "56993921947-t1gh9fque0gbqmcnv8gt3b8dvmprbij9.apps.googleusercontent.com",
                            ClientSecret = "rd5MlgLKb8150_liTviw12sv"
                        },
                        Scopes = new[] { CalendarService.Scope.Calendar },
                        DataStore = new MyDataStore()
                    });

                var tmp =
                    await
                        new AuthorizationCodeWebApp(googleAuthorizationCodeFlow, "", "").AuthorizeAsync(
                            "Adjeko88@gmail.com", new CancellationToken());

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = tmp.Credential,
                    ApplicationName = "TimeMasters Bot"
                });

                return "UserId: " + tmp.Credential.UserId + " AccesToken: " + tmp.Credential.Token.AccessToken +
                    " RefreshToken: " + tmp.Credential.Token.RefreshToken;
            }
            catch (System.Exception ex)
            {
                logger.Error<TestGoogle>("Google fucked up", ex);
            }
            return "nix";
        }
    }

    public class MyDataStore : IDataStore
    {
        private Dictionary<string, object> dic;

        public MyDataStore()
        {
            dic = new Dictionary<string, object>();
        }

        public Task StoreAsync<T>(string key, T value)
        {
            return Task.Run(() => dic.Add(key, value));
        }

        public Task DeleteAsync<T>(string key)
        {
            return Task.Run(() => dic.Remove(key));
        }

        public Task<T> GetAsync<T>(string key)
        {
            object fac;
            dic.TryGetValue(key, out fac);
            return Task.Run(() => (T)fac);
        }

        public Task ClearAsync()
        {
            return Task.Run(() => dic.Clear());
        }
    }
}
