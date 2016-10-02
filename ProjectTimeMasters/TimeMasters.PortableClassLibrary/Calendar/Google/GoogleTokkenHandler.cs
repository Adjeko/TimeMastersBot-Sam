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
        private const string ClientId = "505197371577-pu20ckhu9efr09kst8f5rmob4rssa9vr.apps.googleusercontent.com";
        private const string ClientSecret = "JrpjK2xRj2UDNRGGSM1lKjX7";
        private const string AuthUri = "https://accounts.google.com/o/oauth2/auth";
        private const string TokenUri = "https://accounts.google.com/o/oauth2/token";
        private const string RedirectUri = "http://timemastersweb.azurewebsites.net/Home";
        private const string OriginUri = "http://timemastersbot.azurewebsites.net";


        public string GetAuthenticationRedirectUri()
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
                    new AuthorizationCodeWebApp(flow, RedirectUri, "Bot")
                        .AuthorizeAsync
                        ("Adjeko88@gmail.com", CancellationToken.None);

                result = resultTask.Result;
            }
            catch (System.Exception ex)
            {
                //Logger.GetInstance().Error<TestGoogle>("CodeFlow Exc", ex);
            }
            return result?.RedirectUri;
        }

        public TokenResponse GetAuthorizationTokens(string code)
        {
            TokenResponse tokenResponse = null;

            //string resultString = "";
            try
            {
                var tokenRequest = new AuthorizationCodeTokenRequest();
                tokenRequest.Code = code;
                tokenRequest.GrantType = "authorization_code";
                tokenRequest.RedirectUri = RedirectUri;
                tokenRequest.ClientId = ClientId;
                tokenRequest.ClientSecret = ClientSecret;
                tokenRequest.Scope = CalendarService.Scope.Calendar;

                Task<TokenResponse> tokenResponseTask = tokenRequest.ExecuteAsync(new HttpClient(), "https://accounts.google.com/o/oauth2/token", CancellationToken.None,
                    SystemClock.Default);

                tokenResponse = tokenResponseTask.Result;
                tokenResponse.Issued = DateTime.Now;
                //resultString = $"Access: {tokenResponse.AccessToken} Refresh: {tokenResponse.RefreshToken} LifeTime: {tokenResponse.ExpiresInSeconds} Issued: {tokenResponse.Issued} Scope: {tokenResponse.Scope}";
            }
            catch (System.Exception ex)
            {
                //Logger.GetInstance().Error<TestGoogle>("TestGrant", ex);
            }
            return tokenResponse;
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

        public TokenResponse RenewAccessToken(string refreshToken)
        {
            TokenResponse tokenResponse = null;

            try
            {
                RefreshTokenRequest refreshRequest = new RefreshTokenRequest();
                refreshRequest.RefreshToken = refreshToken;
                refreshRequest.ClientId = ClientId;
                refreshRequest.ClientSecret = ClientSecret;
                refreshRequest.GrantType = "refresh_token";
                refreshRequest.Scope = CalendarService.Scope.Calendar;


                Task<TokenResponse> refreshTask = refreshRequest.ExecuteAsync(new HttpClient(),
                    "https://accounts.google.com/o/oauth2/token", CancellationToken.None,
                    SystemClock.Default);

                tokenResponse = refreshTask.Result;
            }
            catch (System.Exception ex)
            {
                
            }

            return tokenResponse;
        }
    }
}

