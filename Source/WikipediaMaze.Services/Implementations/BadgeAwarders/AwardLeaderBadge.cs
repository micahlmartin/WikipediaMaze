using System;
using System.Collections.Generic;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardLeaderBadge : BaseBadgeAwarder
    {
        protected override UserActionType ActionType
        {
            get { return UserActionType.SolvedPuzzle; }
        }

        protected override BadgeType BadgeType
        {
            get { return Core.BadgeType.Leader; }
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override User GetAffectedUser(UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool ShouldAwardBadge(User user, UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return user.LeadingPuzzleCount >= 5;
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
