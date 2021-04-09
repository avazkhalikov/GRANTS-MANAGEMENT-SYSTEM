using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTAMVC3.Helpers;
using System.Web.Routing;

namespace BOTAMVC3.Controllers
{
    [HandleError]
    public class HomeController : BOTAController
    {
        public ActionResult Index()
        {
            
             //

            // IDictionary<string, object> val = GetRouteInfo(System.Web.HttpContext.Current);

            PageAccess();
             //string controller = val.FirstOrDefault(x => x.Value == "Controller").ToString(); // val.Values["Controller"];
            // string action = val.Values["Action"]; 
            
             return RedirectToAction("Index","Tasks");

        }


        //public static bool IsCurrentAction(this HtmlHelper helper, string actionName, string controllerName)
        //{
        //    string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
        //    string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];

        //    if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) && currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
        //        return true;

        //    return false;
        //}

       

    }
}
