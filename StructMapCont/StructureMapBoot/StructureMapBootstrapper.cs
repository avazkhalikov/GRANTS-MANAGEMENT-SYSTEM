using System;
using System.Configuration;
using StructureMap;
using StructMapCont;
using BOTACORE.CORE.DataAccess;
using BOTACORE.CORE.DataAccess.Impl;
using BOTACORE.CORE.Services;
using BOTACORE.CORE.Services.Impl;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.DAL;


namespace StructMapCont
{
    public class StructureMapBootstrapper : IStructureMapBootstrapper
    {
        private static bool hasStarted;

        public void BootstrapStructureMap()
        {

            var container = new Container(_ =>
            {


                _.For<IOrganizationRepository>().Use<OrganizationRepository>();
                _.For<IProposalRepository>().Use<ProposalRepository>();

                //TODO: Using TestRepository For Now.
                // x.For<IAccountRepository>().Use<AccountRepository>();
                _.For<IAccountRepository>().Use<AccountTestRepository>();
                _.For<IEventTypeRepository>().Use<EventTypeRepository>();
                _.For<IWordTemplateService>().Use<WordTemplateService>();
                _.For<ICryptService>().Use<CryptService>();


                _.For<IAccountService>().Use<AccountService>();
                _.For<IEventTypeService>().Use<EventTypeService>();
                _.For<IProposalService>().Use<ProposalService>();
                _.For<IWebContext>().Use<WebContext>();
                _.For<IUserSession>().Use<UserSession>();
                _.For<IRedirector>().Use<Redirector>();





                // x.For<IConfiguration>().Use<BOTACORE.CORE.Impl.Configuration>();


                _.For<ISqlCommands>().Use<SqlCommands>();



                // x.For<IFunctionState>().AddConcreteType<AlarmFunctionState>();



                //--------------- test staff following.   ----------------
                _.For<IGenericInterface<String>>().Use<GenericClass2>();
                _.For(typeof(IGenericInterface<>)).Use(typeof(GenericClass<>));
                _.For<botaweb.StructureMapGenerics.IHelloWorld>().Use<botaweb.StructureMapGenerics.HelloWorldPrinter>();

                _.For<BOTACORE.CORE.IHelloWorld>().Use<BOTACORE.CORE.Impl.HelloWorldPrinter2>();
                _.For<BOTACORE.CORE.IHelloWorld>().Use<BOTACORE.CORE.Impl.HelloWorldPrinter>();

                                        

                                        //IHelloWorld has 3 implementations: TheDefaultIsConcreteType is the default one.
                                       // x.For<BOTACORE.CORE.IHelloWorld>().TheDefaultIsConcreteType<BOTACORE.CORE.Impl.HelloWorldPrinter>();
                                     //   x.For<BOTACORE.CORE.IHelloWorld>().AddInstances(i => i.OfConcreteType<HelloWorldPrinter2>());
                                    //    x.For<BOTACORE.CORE.IHelloWorld>().AddInstances(i => i.OfConcreteType<HelloWorldPrinter3>());
                                       

                                      }
                                 );

            /* Sample 
            ObjectFactory.Initialize(x =>
            {
                x.For<ISessionHandler>().Use<SessionHandler>();
                x.For<ITempDataHandler>().Use<TempDataHandler>();

                x.ForSingletonOf<ICacheHandler>().Use<CacheHandler>();

                x.AddRegistry<FactoryRegistry>();
                x.AddRegistry<RepositoryRegistry>();
                x.AddRegistry<ServiceRegistry>();
            });
             */
        }

        public static void Restart()
        {
            //if (hasStarted)
            //{
            //    ObjectFactory.ResetDefaults();
            //}
            //else
            //{
                Bootstrap();
            //    hasStarted = true;
            //}
        }

        public static void Bootstrap()
        {
            new StructureMapBootstrapper().BootstrapStructureMap();
        }
    }

}
