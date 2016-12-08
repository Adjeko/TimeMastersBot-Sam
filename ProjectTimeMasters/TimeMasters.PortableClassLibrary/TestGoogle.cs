using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using TimeMasters.PortableClassLibrary.Logging;

//using Google.Apis.Auth.OAuth2.Mvc;

namespace TimeMasters.PortableClassLibrary
{
    public class TestGoogle
    {

        public void Test()
        {
            try
            {
                
            }
            catch (System.Exception ex)
            {
                Logger.GetInstance().Error<TestGoogle>("Google fucked up", ex);
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
