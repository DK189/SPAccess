using SPAccess.ForWebAppMVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo_SPAccess.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test ()
        {
            SPContext.Current.Test();
            return RedirectToAction("Index", "Home");
        }
    }
}