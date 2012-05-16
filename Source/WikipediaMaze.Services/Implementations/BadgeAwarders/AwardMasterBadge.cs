using System;
using System.Collections.Generic;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardMasterBadge : BaseBadgeAwarder
    {
        protected override Core.UserActionType ActionType
        {
            get { return Core.UserActionType.SolvedPuzzle; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Master; }
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override Core.User GetAffectedUser(Core.UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool ShouldAwardBadge(User user, UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return user.LeadingPuzzleCount >= 50;
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
