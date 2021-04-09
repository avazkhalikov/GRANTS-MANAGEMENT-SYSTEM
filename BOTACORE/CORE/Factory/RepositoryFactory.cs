using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using BOTACORE.CORE.DataAccess;
using BOTACORE.CORE.DataAccess.Impl;
using System.Threading;

namespace BOTACORE.CORE
{
    //RepositoryFactory is just a Wrapper or a Capsul that Encapsulates StructureMap.
    //a new instance of AccountRepository returned in every call by StructureMap!
    public class RepositoryFactory
    {
        

        public static IAccountRepository AccountRepository()
        {
            Container container = new Container();
            return container.GetInstance<IAccountRepository>();
            //   return ObjectFactory.GetInstance<IAccountRepository>(); // ObjectFactory.GetInstance<IAccountRepository>();
        }

        public static IEventTypeRepository EventTypeRepository()
        {
            Container container = new Container();
            return container.GetInstance<IEventTypeRepository>();
        }

        public static IIndicatorRepository IndicatorRepository()
        {
            Container container = new Container();
            return container.GetInstance<IIndicatorRepository>();
        }

        public static IProjectEventRepository ProjectEventRepository()
        {
            Container container = new Container();
            return container.GetInstance<IProjectEventRepository>();
        }

        public static IProposalRepository ProposalRepository()
        {
            Container container = new Container();
            return container.GetInstance<IProposalRepository>();
        }


        public static IOrganizationRepository OrganizationRepository()
        {
            Container container = new Container();
            return container.GetInstance<IOrganizationRepository>();
        }

        public static ISSPStaffRepository SSPStaffRepository()
        {
            Container container = new Container();
            return container.GetInstance<ISSPStaffRepository>();
        }

        public static ITemplateRepository TemplateRepository()
        {
            Container container = new Container();
            return container.GetInstance<ITemplateRepository>();
        }

        public static IGrantTypeListRepository GrantTypeListRepository()
        {
            Container container = new Container();
            return container.GetInstance<IGrantTypeListRepository>();
        }

        /*
        public static class ObjF
        {
            public static T Get<T>()
            {
                //IAccountRepository  accrep = new AccountRepository();
                return ObjectFactory.GetInstance<T>();  // return (T)accrep;
            }

            public static T Get<T>(Type type)
            {


                return ObjectFactory.With(type).GetInstance<T>();


            }
        }
         */

    }
}