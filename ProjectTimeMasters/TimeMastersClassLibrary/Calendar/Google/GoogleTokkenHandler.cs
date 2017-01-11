using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TimeMastersClassLibrary.Calendar.Google
{

    public class SerializeContractResolver : DefaultContractResolver
    {
        public new static readonly SerializeContractResolver Instance = new SerializeContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);


            if(property.PropertyName == "UnsuccessfulResponseHandlers")
            {
                property.Ignored = true;
            }

            //if (property.PropertyName == "BackOff"
            //    || property.PropertyName == "MaxTimeSpan"
            //    || property.PropertyName == "HandleUnsuccessfulResponseFunc"
            //    || property.PropertyName == "HandleExceptionFunc")
            //{
            //    property.Ignored = true;
            //}


            //if (property.PropertyName == "UnsuccessfulResponseHandlers"
            //    || property.PropertyName == "SupportsAutomaticDecompression" 
            //    || property.PropertyName == "SupportsProxy" 
            //    || property.PropertyName == "SupportsRedirectConfiguration" 
            //    || property.PropertyName == "UseCookies" 
            //    || property.PropertyName == "CookieContainer" 
            //    || property.PropertyName == "ClientCertificateOptions" 
            //    || property.PropertyName == "AutomaticDecompression" 
            //    || property.PropertyName == "UseProxy" 
            //    || property.PropertyName == "PreAuthenticate" 
            //    || property.PropertyName == "Credentials" 
            //    || property.PropertyName == "AllowAutoRedirect" 
            //    || property.PropertyName == "MaxAutomaticRedirections" 
            //    || property.PropertyName == "MaxRequestContentBufferSize" 
            //    || property.PropertyName == "Proxy" 
            //    || property.PropertyName == "UseDefaultCredentials")
            //{
            //    property.Ignored = true;
            //}
            if (property.DeclaringType == typeof(GoogleAuthorizationCodeFlow) && property.PropertyName == "includeGrantedScopes")
            {
                property.Ignored = true;
            }
            return property;
        }
    }


    /// <summary>
    /// Handles the Google Authorization Process after https://tools.ietf.org/html/rfc6749
    /// </summary>
    public class GoogleTokkenHandler
    {
        public static Dictionary<string, string> UserCodeDictionary = new Dictionary<string, string>();
        public static Dictionary<string, GoogleAuthorizationCodeFlow> UserCodeFlows = new Dictionary<string, GoogleAuthorizationCodeFlow>();
        public static Dictionary<string, TokenResponse> UserTokens = new Dictionary<string, TokenResponse>();

        private const string ClientId = "505197371577-pu20ckhu9efr09kst8f5rmob4rssa9vr.apps.googleusercontent.com";
        private const string ClientSecret = "JrpjK2xRj2UDNRGGSM1lKjX7";
        private const string AuthUri = "https://accounts.google.com/o/oauth2/auth";
        private const string TokenUri = "https://accounts.google.com/o/oauth2/token";
        private const string RedirectUri = "http://timemastersbot.azurewebsites.net/api/Register";
        private const string OriginUri = "http://timemastersbot.azurewebsites.net";

        private static GoogleAuthorizationCodeFlow staticFlow;
        private static TokenResponse staticTokenResponse;
        public string GetAuthenticationRedirectUri(string state)
        {
            Task<AuthorizationCodeWebApp.AuthResult> resultTask = null;
            AuthorizationCodeWebApp.AuthResult result = null;
            GoogleAuthorizationCodeFlow flow;
            try
            {
                flow =
                    new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
                    {
                        ClientSecrets = new ClientSecrets()
                        {
                            ClientId = ClientId,
                            ClientSecret = ClientSecret
                        },
                        Scopes = new[] { CalendarService.Scope.Calendar },
                        //DataStore = new FileDataStore("TimeMasters")
                    });


                resultTask =
                    new AuthorizationCodeWebApp(flow, RedirectUri, state)
                        .AuthorizeAsync
                        ("Adjeko88@gmail.com", CancellationToken.None);

                result = resultTask.Result;
                GoogleTokkenHandler.UserCodeFlows.Add(state, staticFlow);

                if(staticFlow == null)
                {
                    staticFlow = flow;
                }
            }
            catch (System.Exception ex)
            {
                //Logger.GetInstance().Error<TestGoogle>("CodeFlow Exc", ex);
            }
            return $"{result?.RedirectUri}";
        }

        public static CalendarService GetCalendarService(string userId)
        {
            //GoogleAuthorizationCodeFlow codeFlow = UserCodeFlows[userId];
            //TokenResponse tr = UserTokens[userId];
            //string flowString = Newtonsoft.Json.JsonConvert.SerializeObject(codeFlow, new JsonSerializerSettings
            //{
            //    ContractResolver = SerializeContractResolver.Instance,
            //    TypeNameHandling = TypeNameHandling.Objects
            //});
            //if (codeFlow == null)
            //{
            //    TimeMasters.PortableClassLibrary.Logging.Logger.GetInstance().Info<GoogleTokkenHandler>("flow is null");
            //}

            ////if(flowString == "null")
            ////{
            ////    TimeMasters.PortableClassLibrary.Logging.Logger.GetInstance().Info<GoogleTokkenHandler>("flowString is null");
            ////}
            
            //GoogleAuthorizationCodeFlow newFlow = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleAuthorizationCodeFlow>(flowString, new JsonSerializerSettings
            //{
            //    ContractResolver = SerializeContractResolver.Instance,
            //    TypeNameHandling = TypeNameHandling.Objects
            //});

            UserCredential user = new UserCredential(staticFlow, userId, UserTokens[userId]);

            CalendarService service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = user,
                ApplicationName = "TimeMasters Bot"
            });
            return service;
        }


        public bool GetAuthorizationTokens(string code, string userId, out string accessToken, out string refreshToken, out DateTime issued, out long expires)
        {
            staticTokenResponse = null;
            try
            {
                var tokenRequest = new AuthorizationCodeTokenRequest()
                {
                    Code = code,
                    GrantType = "authorization_code",
                    RedirectUri = RedirectUri,
                    ClientId = ClientId,
                    ClientSecret = ClientSecret,
                    Scope = CalendarService.Scope.Calendar
                };
                Task<TokenResponse> tokenResponseTask = tokenRequest.ExecuteAsync(new HttpClient(), "https://accounts.google.com/o/oauth2/token", CancellationToken.None,
                    SystemClock.Default);

                staticTokenResponse = tokenResponseTask.Result;
                staticTokenResponse.Issued = DateTime.Now;
                accessToken = staticTokenResponse.AccessToken;
                refreshToken = staticTokenResponse.RefreshToken;
                issued = staticTokenResponse.Issued;
                expires = (long)staticTokenResponse.ExpiresInSeconds;

                UserTokens.Add(userId, staticTokenResponse);
                return true;
            }
            catch (System.Exception ex)
            {
            }

            accessToken = "";
            refreshToken = "";
            issued = DateTime.MinValue;
            expires = 0;

            return false;
        }

        public void StoreTokens(string userId, TokenResponse tokens)
        {
            //TODO store tokens in database
        }

        public UserCredential GetUserCredentials(string userId)
        {
            //TODO get tokens from database
            // if tokens are expired, renew them
            // Date of creation and lifetime is stored in the database
            // then create UserCredentials
            return null;
        }

        public bool RenewAccessToken(string refreshToken, out string accessToken, out DateTime issued, out long expires)
        {
            TokenResponse tokenResponse = null;
            string error = "";
            try
            {
                RefreshTokenRequest refreshRequest = new RefreshTokenRequest()
                {
                    RefreshToken = refreshToken,
                    ClientId = ClientId,
                    ClientSecret = ClientSecret,
                    GrantType = "refresh_token",
                    Scope = CalendarService.Scope.Calendar
                };
                Task<TokenResponse> refreshTask = refreshRequest.ExecuteAsync(new HttpClient(),
                    "https://accounts.google.com/o/oauth2/token", CancellationToken.None,
                    SystemClock.Default);

                tokenResponse = refreshTask.Result;

                accessToken = tokenResponse.AccessToken;
                issued = tokenResponse.Issued;
                expires = tokenResponse.ExpiresInSeconds.GetValueOrDefault();
                return true;
            }
            catch (System.Exception ex)
            {
                //catch something
                error = ex.Message + "\n\n" + ex.InnerException.Message;
            }

            accessToken = error;
            issued = new DateTime();
            expires = 0;
            return false;
        }
    }
}

