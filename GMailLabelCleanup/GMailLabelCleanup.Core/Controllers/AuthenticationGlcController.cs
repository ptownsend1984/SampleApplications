using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GMailLabelCleanup.Core.Managers;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace GMailLabelCleanup.Core.Controllers
{
    public abstract class AuthenticationGlcController : GlcController
    {

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return SignInManager.AuthenticationManager;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

    }
}