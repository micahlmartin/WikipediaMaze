using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardAddictBadge : AwardBadgeBase
    {
        public AwardAddictBadge(IRepository repository) : base(repository) { }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override bool ShouldAwardBadge(User user)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30).Date;
            var actions = Repository.All<UserAction>().Where(x => x.UserId == user.Id && x.DateCreated <= thirtyDaysAgo && x.Action == UserActionType.SolvedPuzzle);

            var dateToCheck = thirtyDaysAgo;
            var awardBadge = true;
            var today = DateTime.UtcNow.Date;

            while (dateToCheck < today && awardBadge)
            {
                if (actions.Any(x => x.DateCreated.Date == dateToCheck))
                    awardBadge = false;

                dateToCheck = dateToCheck.AddDays(1);
            }

            return awardBadge;
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Addict; }
        }
    }
}
