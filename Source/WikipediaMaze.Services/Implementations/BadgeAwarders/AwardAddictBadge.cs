using System;
using System.Linq;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardAddictBadge : BaseBadgeAwarder
    {
        protected override UserActionType ActionType
        {
            get { return UserActionType.SolvedPuzzle; }
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Addict; }
        }

        protected override bool ShouldAwardBadge(Core.User user, Core.UserAction action)
        {
            var numberOfDaysToCheck = 30;
            var now = DateTime.UtcNow;
            var actions = Repository.All<UserAction>().Where(x => x.UserId == user.Id && x.DateCreated > now.AddDays(numberOfDaysToCheck * -1) && x.Action == UserActionType.SolvedPuzzle);

            var awardBadge = true;
            var counter = 0;

            while (counter < numberOfDaysToCheck && awardBadge)
            {
                var dateToCheck = now.AddDays(counter * -1).Date;
                if (!actions.Any(x => x.DateCreated.Date == dateToCheck))
                    awardBadge = false;

                counter++;
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
