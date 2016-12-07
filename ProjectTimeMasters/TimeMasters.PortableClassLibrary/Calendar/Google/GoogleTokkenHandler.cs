using System;
using System.Collections.Generic;
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
//using TimeMasters.PortableClassLibrary.Logging;

namespace TimeMasters.PortableClassLibrary.Calendar.Google
{
    /// <summary>
    /// Handles the Google Authorization Process after https://tools.ietf.org/html/rfc6749
    /// </summary>
    public class GoogleTokkenHandler
    {
        public static Dictionary<string, string> UserCodeDictionary = new Dictionary<string, string>();


        private const string ClientId = "505197371577-pu20ckhu9efr09kst8f5rmob4rssa9vr.apps.googleusercontent.com";
        private const string ClientSecret = "JrpjK2xRj2UDNRGGSM1lKjX7";
        private const string AuthUri = "https://accounts.google.com/o/oauth2/auth";
        private const string TokenUri = "https://accounts.google.com/o/oauth2/token";
        private const string RedirectUri = "http://timemastersbot.azurewebsites.net/api/Register";
        private const string OriginUri = "http://timemastersbot.azurewebsites.net";


        public string GetAuthenticationRedirectUri(string state)
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
            }
            catch (System.Exception ex)
            {
                //Logger.GetInstance().Error<TestGoogle>("CodeFlow Exc", ex);
            }
            return $"{result?.RedirectUri}";
        }

        public bool GetAuthorizationTokens(string code, out string accessToken, out string refreshToken, out DateTime issued, out long expires)
        {
            TokenResponse tokenResponse = null;
            bool success = true;
            //string resultString = "";
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

                tokenResponse = tokenResponseTask.Result;
                tokenResponse.Issued = DateTime.Now;
                //resultString = $"Access: {tokenResponse.AccessToken} Refresh: {tokenResponse.RefreshToken} LifeTime: {tokenResponse.ExpiresInSeconds} Issued: {tokenResponse.Issued} Scope: {tokenResponse.Scope}";
            }
            catch (System.Exception ex)
            {
                //Logger.GetInstance().Error<TestGoogle>("TestGrant", ex);
                success = false;
            }

            accessToken = tokenResponse.AccessToken;
            refreshToken = tokenResponse.RefreshToken;
            issued = tokenResponse.Issued;
            expires = (long)tokenResponse.ExpiresInSeconds;

            return success;
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

