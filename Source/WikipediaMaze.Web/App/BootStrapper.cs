using System;
using System.Collections.Generic;
using StructureMap;
using WikipediaMaze.Data;
using WikipediaMaze.Data.Mongo;
using WikipediaMaze.Data.NHibernate;
using NHibernate;
using FluentNHibernate.Cfg;
using StructureMap.Configuration.DSL;
using WikipediaMaze.Services;
using WikipediaMaze.Services.Implementations.BadgeAwarders;
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
                //Data
                ForRequestedType<IRepository>().TheDefaultIsConcreteType<MongoRepository>();

                //Services
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
                ForRequestedType<IWebSnapshotService>().TheDefaultIsConcreteType<WebSnapshotService>();
                
                //Recurring Services
                ForRequestedType<IRecurringService>().AddConcreteType<AwardBadgesService>();
                ForRequestedType<IRecurringService>().AddConcreteType<ThemeCountUpdateService>();

                //Badge Awarders
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardAddictBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardBetaBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardCrazedBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardCreatorBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardCriticBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardDominatorBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardEnigmatistBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardFamousBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardLeaderBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardMasterBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardMysterioBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardNotableBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardPlayerBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardPopularBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardRiddlerBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardSupporterBadge>();
                ForRequestedType<IAwardBadge>().AddConcreteType<AwardYearlingBadge>();
            }   
        }

        private static void ConfigureLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion
    }
}
