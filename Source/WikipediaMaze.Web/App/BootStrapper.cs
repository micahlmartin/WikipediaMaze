using System.Collections.Generic;
using StructureMap;
using WikipediaMaze.Data;
using WikipediaMaze.Data.Mongo;
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
                For<IRepository>().Use<MongoRepository>();

                //Services
                For<ITwitterService>().Use<TwitterService>();
                For<IMessengerService>().Use<MessengerService>();
                For<IReputationService>().Use<ReputationService>();
                For<ITopicCache>().Use<TopicCache>();
                For<IGameService>().Use<GameService>();
                For<ITopicService>().Use<TopicService>();
                For<IPuzzleService>().Use<PuzzleService>();
                For<IAuthenticationService>().Use<FormsAuthenticationService>();
                For<IAccountService>().Use<AccountService>();
                For<IPuzzleCache>().Use<PuzzleCache>();
                For<IWebSnapshotService>().Use<WebSnapshotService>();
                
                //Recurring Services
                For<IRecurringService>().Use<AwardBadgesService>();
                For<IRecurringService>().Use<ThemeCountUpdateService>();

                //Badge Awarders
                For<IAwardBadge>().Use<AwardAddictBadge>();
                For<IAwardBadge>().Use<AwardBetaBadge>();
                For<IAwardBadge>().Use<AwardCrazedBadge>();
                For<IAwardBadge>().Use<AwardCreatorBadge>();
                For<IAwardBadge>().Use<AwardCriticBadge>();
                For<IAwardBadge>().Use<AwardDominatorBadge>();
                For<IAwardBadge>().Use<AwardEnigmatistBadge>();
                For<IAwardBadge>().Use<AwardFamousBadge>();
                For<IAwardBadge>().Use<AwardLeaderBadge>();
                For<IAwardBadge>().Use<AwardMasterBadge>();
                For<IAwardBadge>().Use<AwardMysterioBadge>();
                For<IAwardBadge>().Use<AwardNotableBadge>();
                For<IAwardBadge>().Use<AwardPlayerBadge>();
                For<IAwardBadge>().Use<AwardPopularBadge>();
                For<IAwardBadge>().Use<AwardRiddlerBadge>();
                For<IAwardBadge>().Use<AwardSupporterBadge>();
                For<IAwardBadge>().Use<AwardYearlingBadge>();
            }   
        }

        private static void ConfigureLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion
    }
}
