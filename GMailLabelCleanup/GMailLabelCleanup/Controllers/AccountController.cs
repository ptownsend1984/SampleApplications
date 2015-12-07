using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2.Flows;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using GMailLabelCleanup.Core.ViewModels.Account;
using GMailLabelCleanup.Data.Models.Identity;
using GMailLabelCleanup.Data;
using Microsoft.ApplicationInsights;
using GMailLabelCleanup.Core.Controllers;
using GMailLabelCleanup.Core.Components;
using Google.Apis.Auth.OAuth2.Responses;

namespace GMailLabelCleanup.Controllers
{
    [Authorize]
    public class AccountController : AuthenticationGlcController
    {

        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            await Task.Run(() => new TelemetryClient().TrackEvent("ExternalLoginCallback"));

            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindAsync(loginInfo.Login);
                    if (user != null)
                    {
                        await StoreAuthToken(user.Id, user.UserName);
                    }
                    await Task.Run(() => new TelemetryClient().TrackEvent("LogIn"));
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return RedirectToAction("Login");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("Login");
                case SignInStatus.Failure:
                default:
                    return await CreateExternalUser(loginInfo, returnUrl);
            }
        }

        private async Task<ActionResult> CreateExternalUser(ExternalLoginInfo loginInfo, string returnUrl)
        {
            // If the user does not have an account, then prompt the user to create an account
            var user = new ApplicationUser { UserName = loginInfo.Email, Email = loginInfo.Email, DateCreatedUtc = DateTime.UtcNow };
            var result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            await StoreAuthToken(user.Id, user.UserName);

            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            await Task.Run(() => new TelemetryClient().TrackEvent("CreateUser"));

            return RedirectToLocal(returnUrl);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            this.AuthenticationManager.SignOut();

            await Task.Run(() => new TelemetryClient().TrackEvent("LogOut"));

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginFailure()
        {
            await Task.Run(() => new TelemetryClient().TrackEvent("ExternalLoginFailure"));

            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private async Task StoreAuthToken(string userId, string userName)
        {
            //Look for the claims stored in GoogleOAuth2AuthenticationProvider.OnAuthenticated
            var claimsIdentity = await this.AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            if (claimsIdentity == null)
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "No claims identity.");

            var accessToken = claimsIdentity.FindAll("accesstoken").FirstOrDefault();
            if (accessToken == null)
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "No access token claim.");

            var expiresIn = claimsIdentity.FindAll("expiresin").FirstOrDefault();
            if (expiresIn == null)
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "No access token expiration claim.");

            var refreshtoken = claimsIdentity.FindAll("refreshtoken").FirstOrDefault();
            if (refreshtoken == null)
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "No refresh token claim.");

            long expiresInValue;
            long.TryParse(expiresIn.Value, out expiresInValue);

            await StoreAuthToken(userId, userName, refreshtoken.Value, accessToken.Value, expiresInValue);
        }
        private async Task StoreAuthToken(string userId, string userName, string refreshToken, string accessToken, long expiresIn)
        {
            var flow = GmailApiComponent.CreateFlow(userId);
            var token = await flow.DataStore.GetAsync<TokenResponse>(userName);
            if (token == null && string.IsNullOrEmpty(refreshToken))
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "No refresh token provided.");

            if (token == null)
            {
                token = new Google.Apis.Auth.OAuth2.Responses.TokenResponse
                {
                    RefreshToken = refreshToken
                };
            }
            token.AccessToken = accessToken;
            token.ExpiresInSeconds = expiresIn;
            token.Issued = flow.Clock.UtcNow;

            await flow.DataStore.StoreAsync(userName, token);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}