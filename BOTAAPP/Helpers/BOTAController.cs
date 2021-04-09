using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Services.Impl;
using BOTACORE.CORE.Services;
using System.Web.Routing;
using BOTACORE.CORE.Domain; 
namespace BOTAMVC3.Helpers
{
    public class BOTAController:Controller
    {
        
        public IUserSession session;
        public BOTAController()
        {
            session = new UserSession();
            if (!session.LoggedIn)
            {
                System.Web.HttpContext.Current.Response.Redirect("/SSPStaff/Login");
            }
             
        }

        public int PageAccess()
        {
            return 2;
            /*
            //Get Current User
            //1. Get Role.            
            session = new UserSession();
            int UserRoleID = session.CurrentUser.RoleID;

            SSPStaffService ss = new SSPStaffService();
            //Get Access List by Role. 
            List<PageAccess> pas = ss.GetPageAccessByRole(UserRoleID); 

            //2. get Current Controller and Action
            string controller = GetRouteInfo<String>(System.Web.HttpContext.Current, "Controller");
            string action = GetRouteInfo<String>(System.Web.HttpContext.Current, "Action");

            //3. select access by controller/action 
            var pa = pas.FirstOrDefault(x => x.Controller.Equals(controller) && x.Action.Equals(action));

            if (pa != null)
            {
                return pa.AccessID;
            }
            else
            {
                return 0; 
            }
             */
        }
        // private static IDictionary<string, object> _values;
        /// <summary>
        /// This one pulls from context ROUTE INFO.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetRouteInfo(HttpContext context)
        {
            IDictionary<string, object> _values;
            HttpContextBase contextBase = new HttpContextWrapper(context);
            RouteData data = RouteTable.Routes.GetRouteData(contextBase);
            RequestContext requestContext = new RequestContext(contextBase, data);
            _values = requestContext.RouteData.Values;

            return _values;
        }

        //PULLS from DICTIONARY NEEDED VALS!
        public T GetRouteInfo<T>(HttpContext context, string key)
        {
            IDictionary<string, object> data = GetRouteInfo(context);

            if (data[key] == null)
                return default(T);

            object objValue = data[key];
            // It appears that route values are all strings, so convert the object to a string.
            if (typeof(T) == typeof(int))
            {
                objValue = int.Parse(data[key].ToString());
            }
            else if (typeof(T) == typeof(long))
            {
                objValue = long.Parse(data[key].ToString());
            }
            else if (typeof(T) == typeof(Guid))
            {
                objValue = new Guid(data[key].ToString());
            }
            return (T)objValue;
        }
        protected override /*virtual*/ void OnActionExecuting(ActionExecutingContext filterContext)
        {
            session = new UserSession();
            if (!session.LoggedIn)
            {
                filterContext.Result = new RedirectResult("/SSPStaff/Login");
            }

            int pa = PageAccess();
            if (pa <= 0)
            { 
               //No Access 
                filterContext.Result = new RedirectResult("/Error/noaccess");
            }

            ViewData["access"] = pa;  //goes to masterPage.
        }
        

    }
}
