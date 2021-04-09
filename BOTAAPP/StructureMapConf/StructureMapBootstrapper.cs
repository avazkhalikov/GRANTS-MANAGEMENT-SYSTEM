using System;
using System.Configuration;
using StructureMap;
using StructureMap.Configuration.DSL;  //this include for Registry.
using StructureMap.Configuration;
using StructMapCont;
using BOTACORE.CORE.DataAccess;
using BOTACORE.CORE.DataAccess.Impl;
using BOTACORE.CORE.Services;
using BOTACORE.CORE.Services.Impl;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.DAL;


namespace StructMapContMVC
{

   public class StructureMapBootstrapper : IStructureMapBootstrapper
    {
        private static bool hasStarted;

        public void BootstrapStructureMap()
        {

            var container = new Container(x =>
            {

                x.For<IOrganizationRepository>().Use<OrganizationRepository>();
                              x.For<IProposalRepository>().Use<ProposalRepository>();
                              x.For<IEventTypeRepository>().Use<EventTypeRepository>();
                              //TODO: Using TestRepository For Now.
                              // x.For<IAccountRepository>().Use<AccountRepository>();
                              x.For<IAccountRepository>().Use<AccountTestRepository>();
                              x.For<IIndicatorRepository>().Use<IndicatorRepository>();
                              x.For<IIndicatorService>().Use<IndicatorService>();
                              x.For<IProjectEventRepository>().Use<ProjectEventRepository>();
                              
                            //  x.For<IProjectRepository>().Use<ProjectRepository>();

                              x.For<ITemplateRepository>().Use<TemplateRepository>();
                              x.For<IWordTemplateService>().Use<WordTemplateService>();
                              x.For<ICryptService>().Use<CryptService>();
                              x.For<IProjectEventService>().Use<ProjectEventService>();
                              x.For<ISSPStaffRepository>().Use<SSPStaffRepository>();
                              x.For<ISSPStaffService>().Use<SSPStaffService>();

                              x.For<IEventTypeService>().Use<EventTypeService>();
                              x.For<IAccountService>().Use<AccountService>();
                              x.For<IProposalService>().Use<ProposalService>();
                              x.For<ITemplateService>().Use<TemplateService>();
                
                              x.For<IWebContext>().Use<WebContext>();

                              x.For<IUserSession>().Use<UserSession>();
                              x.For<IRedirector>().Use<Redirector>();
                              x.For<IGrantTypeListRepository>().Use<GrantTypeListRepository>();
                              x.For<IGrantTypeListService>().Use<GrantTypeListService>();
                            
                               
                               

                            // x.For<IConfiguration>().Use<BOTACORE.CORE.Impl.Configuration>();
                             

                             x.For<ISqlCommands>().Use<SqlCommands>();



                             // x.For<IFunctionState>().AddConcreteType<AlarmFunctionState>();
                         
                                         
                                         
                                         //--------------- test staff following.   ----------------
                                        x.For<IGenericInterface<String>>().Use<GenericClass2>();
                                        x.For(typeof(IGenericInterface<>)).Use(typeof(GenericClass<>));
                                        x.For<botaweb.StructureMapGenerics.IHelloWorld>().Use<botaweb.StructureMapGenerics.HelloWorldPrinter>();

                                        x.For<BOTACORE.CORE.IHelloWorld>().Use<BOTACORE.CORE.Impl.HelloWorldPrinter2>();
                                        x.For<BOTACORE.CORE.IHelloWorld>().Use<BOTACORE.CORE.Impl.HelloWorldPrinter>();

                                        

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
