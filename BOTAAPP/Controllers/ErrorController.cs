using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOTAMVC3.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index(string error, string backLink)
        {
            ViewData["backLink"] = backLink;
            ViewData["error"] = error;
            return View();
        }

        public ActionResult NoAccess(string error, string backLink)
        {
            ViewData["backLink"] = backLink;
            ViewData["error"] = "Sorry, you don't have an access to this page!";
            return View();
        }

       
    }
}
