using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GMailLabelCleanup.Core.ViewModels.Manage;
using GMailLabelCleanup.Data.Models.Identity;
using GMailLabelCleanup.Data;
using Microsoft.ApplicationInsights;
using GMailLabelCleanup.Core.Controllers;
using GMailLabelCleanup.Core.Components;
using System.Data.Entity;
using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2.Responses;
using System.Threading;

namespace GMailLabelCleanup.Controllers
{
    [Authorize(Order = 0)]
    public class ManageController : AuthenticationGlcController
    {

        public ManageController()
        {
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.DeleteAccountError ? "An error occurred deleting the account."
                : "";

            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            var model = new IndexViewModel
            {
                DateCreated = this.GetLocalTime(user.DateCreatedUtc)
            };
            return View(model);
        }

        public async Task<ActionResult> DeleteAccount()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            await RevokeToken(userId);
            var result = await UserManager.DeleteAsync(user);

            await Task.Run(() => new TelemetryClient().TrackEvent("DeleteAccount"));

            if (result.Succeeded)
            {
                this.AuthenticationManager.SignOut();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", new { message = ManageMessageId.DeleteAccountError });
            }
        }
        private async Task RevokeToken(string userId)
        {
            var typeName = typeof(TokenResponse).FullName;

            var context = this.DbContext;
            var tokens = await context.GoogleAuthData
                .Where(o => o.UserId == userId && o.Type == typeName)
                .ToArrayAsync();

            foreach (var item in tokens)
            {
                try
                {
                    var token = JsonConvert.DeserializeObject<TokenResponse>(item.Value);
                    var flow = GmailApiComponent.CreateFlow(userId);
                    var userCredential = new Google.Apis.Auth.OAuth2.UserCredential(flow, userId, token);

                    //Refresh the access token so we can promptly destroy it.
                    await userCredential.RefreshTokenAsync(CancellationToken.None);
                    await userCredential.RevokeTokenAsync(CancellationToken.None);
                }
                catch
                {
                    //TODO: Logging!
                }
            }
        }

        #region Helpers

        public enum ManageMessageId
        {
            Error,
            DeleteAccountError
        }

        #endregion
    }
}