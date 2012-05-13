using System;
using System.Linq;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardCrazedBadge : BaseBadgeAwarder
    {
        protected override Core.UserActionType ActionType
        {
            get { return UserActionType.SolvedPuzzle; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Crazed; }
        }

        protected override bool ShouldAwardBadge(Core.User user, Core.UserAction action)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-60).Date;
            var actions = Repository.All<UserAction>().Where(x => x.UserId == user.Id && x.DateCreated <= thirtyDaysAgo && x.Action == UserActionType.SolvedPuzzle);

            var dateToCheck = thirtyDaysAgo;
            var awardBadge = true;
            var today = DateTime.UtcNow.Date;

            while (dateToCheck < today && awardBadge)
            {
                DateTime check = dateToCheck;
                if (actions.Any(x => x.DateCreated.Date == check))
                    awardBadge = false;

                dateToCheck = dateToCheck.AddDays(1);
            }

            return awardBadge;
        }

        protected override Core.User GetAffectedUser(Core.UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }
    }
}
