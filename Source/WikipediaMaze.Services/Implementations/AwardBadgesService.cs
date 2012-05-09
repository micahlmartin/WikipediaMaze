using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using StructureMap;
using WikipediaMaze.Data;
using WikipediaMaze.Core;
using WikipediaMaze.Services.Interfaces;

namespace WikipediaMaze.Services
{
    public class AwardBadgesService : RecurringServiceBase
    {
        private readonly IRepository _repository;
        private readonly IEnumerable<IAwardBadge> _badgeAwarders;

        public AwardBadgesService(IRepository repository, IEnumerable<IAwardBadge> badgeAwarders)
        {
            _repository = repository;
            _badgeAwarders = badgeAwarders;
        }

        /// <summary>
        /// The unique name of the service.
        /// </summary>
        public override string ServiceName
        {
            get { return "Award Badge Service"; }
        }

        public override int Interval
        {
            get
            {
                return Settings.AwardBadgeInterval;
            }
            set
            {
                base.Interval = value;
            }
        }

        /// <summary>
        /// Awards the badges
        /// </summary>
        protected override void DoWork()
        {
            foreach (var action in _repository.All<UserAction>().Where(x => !x.HasBeenChecked))
            {
                var currentAction = action;
                if (currentAction.AffectedUserId.HasValue)
                {
                    _badgeAwarders.AsParallel().ForAll(x => x.AwardBadge(currentAction.AffectedUserId.Value));
                }

                currentAction.HasBeenChecked = true;
                _repository.Save(action);
            }
        }
    }
}
