using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using System.Configuration;

namespace GMailLabelCleanup.Core.Attributes.ActionFilters
{
    public class AppSettingsAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Insights = ConfigurationManager.AppSettings["Insights"];

            base.OnActionExecuting(filterContext);
        }

    }
}