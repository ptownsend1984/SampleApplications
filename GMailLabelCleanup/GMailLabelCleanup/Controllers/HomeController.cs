using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GMailLabelCleanup.Core.Controllers;

namespace GMailLabelCleanup.Controllers
{

    public class HomeController : GlcController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}