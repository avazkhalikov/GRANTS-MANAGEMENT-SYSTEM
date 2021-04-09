using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using BOTACORE.CORE.Services.Impl;
using BOTACORE.CORE.Services;

namespace BOTACORE.CORE
{
   public class ServiceFactory
    {

       public static IAccountService AccountService()
       {
            Container container = new Container();
            return container.GetInstance<IAccountService>(); // ObjectFactory.GetInstance<IAccountRepository>();
       }
       public static IEventTypeService EventTypeService()
       {
            Container container = new Container();
            return container.GetInstance<IEventTypeService>();
       }
       public static IWordTemplateService WordTemplateService()
       {
            Container container = new Container();
            return container.GetInstance<IWordTemplateService>();
       }

       public static IRedirector Redirector()
       {
            Container container = new Container();
            return container.GetInstance<IRedirector>(); // ObjectFactory.GetInstance<IAccountRepository>();
       }

       public static IProposalService ProposalService()
       {
            Container container = new Container();
            return container.GetInstance<IProposalService>(); // ObjectFactory.GetInstance<IAccountRepository>();
       }

       public static IWebContext WebContext()
       {
            Container container = new Container();
            return container.GetInstance<IWebContext>(); // ObjectFactory.GetInstance<IAccountRepository>();
       }

       public static IUserSession UserSession()
       {

            //var container = new Container(new MvcDemoRegistry());
            // var cultureProvider = container.GetInstance<IProvideCultureInfo>();

            //  var container = new Container();
            // return container.GetInstance<IUserSession>(); 
            Container container = new Container();
            return container.GetInstance<IUserSession>(); 
       }
    }
}
