using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using StructureMap;
using StructMapCont;
using System.Web.Routing;

namespace BOTAMVC.Controllers
{
    public class StructureMapController : DefaultControllerFactory
    {
        public class StructureMapControllerFactory : DefaultControllerFactory
        {
            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                Container container = new Container();
                return container.GetInstance (controllerType) as IController;
            }
        }
    }
}
