using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;
using GMailLabelCleanup.Data.Models.Identity;
using GMailLabelCleanup.Data;
using GMailLabelCleanup.Core.Managers;

namespace GMailLabelCleanup
{
    public partial class Startup
    {

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnException = (context) => { },
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var googleOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"]
            };

            //Must specify 'openid email profile' scopes and the app must enable the Google+ API
            //to use the MVC Google OAuth library.
            googleOptions.Scope.Add("openid email profile");

            //Need GmailModify to change labels.
            googleOptions.Scope.Add(GmailService.Scope.GmailModify);
            googleOptions.Provider = new GoogleOAuth2AuthenticationProvider()
            {
                OnAuthenticated = (context) =>
                {
                    context.Identity.AddClaim(new Claim("name", context.Name));                    
                    context.Identity.AddClaim(new Claim("email", context.Email));
                    context.Identity.AddClaim(new Claim("expiresin", ((int)(context.ExpiresIn.GetValueOrDefault().TotalSeconds)).ToString()));
                    context.Identity.AddClaim(new Claim("accesstoken", context.AccessToken));
                    context.Identity.AddClaim(new Claim("refreshtoken", !string.IsNullOrEmpty(context.RefreshToken) ? context.RefreshToken : string.Empty));
                    return Task.FromResult(0);
                },
            };

            //Request the refresh token by specifying offline.
            //Will only arrive the first time the app is authorized.
            googleOptions.AccessType = "offline";

            googleOptions.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
            app.UseGoogleAuthentication(googleOptions);
        }
    }
}