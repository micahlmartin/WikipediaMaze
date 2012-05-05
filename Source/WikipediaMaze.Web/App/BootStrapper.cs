using System;
using StructureMap;
using WikipediaMaze.Data;
using WikipediaMaze.Data.Mongo;
using WikipediaMaze.Data.NHibernate;
using NHibernate;
using FluentNHibernate.Cfg;
using StructureMap.Configuration.DSL;
using WikipediaMaze.Services;
using WikipediaMaze.Services.Interfaces;
using WikipediaMaze.Services.Implementations;

namespace WikipediaMaze.App
{
    public class Bootstrapper : IBootstrapper
    {
        #region IBootstrapper Members

        public void BootstrapStructureMap()
        {
            ConfigureLogger();
            ObjectFactory.Initialize(x => x.AddRegistry(new ServiceRegistry()));
        }

        private class ServiceRegistry : Registry
        {
            public ServiceRegistry()
            {
                ForRequestedType<ITwitterService>().TheDefaultIsConcreteType<TwitterService>();
                ForRequestedType<IMessengerService>().TheDefaultIsConcreteType<MessengerService>();
                ForRequestedType<IReputationService>().TheDefaultIsConcreteType<ReputationService>();
                ForRequestedType<ITopicCache>().TheDefaultIsConcreteType<TopicCache>();
                ForRequestedType<IGameService>().TheDefaultIsConcreteType<GameService>();
                ForRequestedType<ITopicService>().TheDefaultIsConcreteType<TopicService>();
                ForRequestedType<IPuzzleService>().TheDefaultIsConcreteType<PuzzleService>();
                ForRequestedType<IAuthenticationService>().TheDefaultIsConcreteType<FormsAuthenticationService>();
                ForRequestedType<IAccountService>().TheDefaultIsConcreteType<AccountService>();
                ForRequestedType<IPuzzleCache>().TheDefaultIsConcreteType<PuzzleCache>();
                ForRequestedType<IRepository>().TheDefaultIsConcreteType<MongoRepository>();
                ForRequestedType<IWebSnapshotService>().TheDefaultIsConcreteType<WebSnapshotService>();
                //ForRequestedType<ISessionFactory>().TheDefault.Is.Object(SessionFactory);
                ForRequestedType<IRecurringService>().AddConcreteType<AwardBadgesService>();
                ForRequestedType<IRecurringService>().AddConcreteType<ThemeCountUpdateService>();
                //ForRequestedType<IRecurringService>().AddConcreteType<PuzzleLeaderUpdateService>();
                //ForRequestedType<IRecurringService>().AddConcreteType<UpdatePuzzleSolutionCountService>();
            }
        }

//        [ThreadStatic]
//        private static ISessionFactory _sessionFactory;

//        [ThreadStatic]
//        private static bool _sessionInitialized;
        
//        private static ISessionFactory SessionFactory
//        {
//            get
//            {
//                if (!_sessionInitialized)
//                {
//                    _sessionInitialized = true;

//#if HOMETEST

//                                _sessionFactory =  Fluently.Configure()
//                                    .Database(
//                                    FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2005.ConnectionString(db => db.Is(Settings.HomeTestDBConnection)
//                                        )).Mappings(m => m.FluentMappings.AddFromAssemblyOf<IRepository>())
//                                    .BuildSessionFactory();

//#elif WORKTEST

//                                  _sessionFactory =  Fluently.Configure()
//                                    .Database(
//                                    FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2005.ConnectionString(db => db.Is(Settings.WorkTestDBConnection)
//                                        )).Mappings(m => m.FluentMappings.AddFromAssemblyOf<IRepository>())
//                                    .BuildSessionFactory();

//#else

//                    _sessionFactory = Fluently.Configure()
//                       .Database(
//                        FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2005.ConnectionString(db => db.Is(Settings.WikipediaMazeConnection)
//                            )).Mappings(m => m.FluentMappings.AddFromAssemblyOf<IRepository>())
//                        .BuildSessionFactory();

//#endif
//                }
//                return _sessionFactory;
//            }
//        }

        private static void ConfigureLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion
    }
}
