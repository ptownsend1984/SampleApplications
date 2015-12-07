using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Mvc;
using GMailLabelCleanup.Core.Managers;
using Microsoft.AspNet.Identity.Owin;
using GMailLabelCleanup.Data;
using Microsoft.AspNet.Identity;
using System.Configuration;

namespace GMailLabelCleanup.Core.Controllers
{
    public abstract class GlcController : Controller
    {

        #region Properties

        private ApplicationUserManager _userManager;
        private ApplicationDbContext _dbContext;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        public ApplicationDbContext DbContext
        {
            get
            {
                return _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            set
            {
                _dbContext = value;
            }
        }

        #endregion

        #region Methods

        protected string GetUserId()
        {
            return Request.IsAuthenticated ? User.Identity.GetUserId() : string.Empty;
        }
        protected string GetUserName()
        {
            return Request.IsAuthenticated ? User.Identity.GetUserName() : string.Empty;
        }
        protected string GetEnvironmentMode()
        {
            return ConfigurationManager.AppSettings["EnvironmentMode"];
        }

        /// <summary>
        /// Subtract the timezone offset cookie from the provided DateTime value.
        /// </summary>
        /// <param name="dateTimeUtc"></param>
        /// <returns></returns>
        protected DateTime GetLocalTime(DateTime dateTimeUtc)
        {
            return dateTimeUtc.AddMinutes(-GetTimezoneOffset());
        }

        /// <summary>
        /// Returns the javascript value of Date.getTimezoneOffset stored in a cookie.
        /// </summary>
        /// <returns></returns>
        protected int GetTimezoneOffset()
        {
            //Pull timezoneOffset cookie and divide the value by 60
            //since Javascript getTimezoneOffset is in minutes.
            int value;
            var cookie = this.Request.Cookies["timezoneOffset"];
            return cookie != null && int.TryParse(cookie.Value, out value) ? value : 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
            
            base.Dispose(disposing);
        }

        #endregion

    }
}