using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using GMailLabelCleanup.Core.Models;
using System.Configuration;
using System.Threading.Tasks;
using Google;
using System.Threading;
using System.Web;

namespace GMailLabelCleanup.Core.Components
{
    public class GmailApiComponent
    {

        #region Static Members

        private static Random _random = new Random();

        private static ClientSecrets _clientSecrets = new ClientSecrets
        {
            ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
            ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"]
        };
        public static IAuthorizationCodeFlow CreateFlow(string userId)
        {
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = _clientSecrets,
                Scopes = new[] { GmailService.Scope.GmailModify },
                DataStore = new GoogleAuthDataStore(userId)
            };
            return new GoogleAuthorizationCodeFlow(initializer);
        }

        #endregion

        #region Methods

        public async Task<GmailService> GetGmailService(string userId, string userName)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");

            var flow = CreateFlow(userId);
            var token = await flow.LoadTokenAsync(userName, CancellationToken.None);
            if (token == null)
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "No stored token available.");

            var credentials = new UserCredential(flow, userName, token);
            return new GmailService(new Google.Apis.Services.BaseClientService.Initializer
            {
                ApplicationName = "Gmail Label Cleanup",
                HttpClientInitializer = credentials
            });
        }

        public async Task ExecuteBackoffAsync(Action action)
        {
            GoogleApiException lastError = null;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch (GoogleApiException ex)
                {
                    if (ex.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        lastError = ex;
                    }
                    else
                        throw;
                }
                await Task.Delay(1 << i * 1000 + _random.Next(1, 500)).ConfigureAwait(false);
            }
            throw lastError; //Never null
        }
        public async Task<T> ExecuteBackoffAsync<T>(Func<T> func)
        {
            GoogleApiException lastError = null;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    return func();
                }
                catch (GoogleApiException ex)
                {
                    if (ex.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        lastError = ex;
                    }
                    else
                        throw;
                }
                await Task.Delay(1 << i * 1000 + _random.Next(1, 500)).ConfigureAwait(false);
            }
            throw lastError; //Never null
        }

        #endregion

    }
}