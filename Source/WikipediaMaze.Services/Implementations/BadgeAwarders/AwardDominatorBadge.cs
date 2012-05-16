using System;
using System.Collections.Generic;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardDominatorBadge : BaseBadgeAwarder
    {
        protected override UserActionType ActionType
        {
            get { return UserActionType.SolvedPuzzle; }
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Dominator; }
        }

        protected override User GetAffectedUser(Core.UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool ShouldAwardBadge(User user, UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return user.LeadingPuzzleCount >= 25;
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override BadgeAwardInfo GetBadgeAwardInfo(UserAction action)
        {
            return new BadgeAwardInfo
            {
                DateAwarded = action.DateCreated
            };
        }
    }
}
