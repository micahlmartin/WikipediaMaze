using System;
using System.Collections.Generic;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardCreatorBadge : BaseBadgeAwarder
    {
        protected override UserActionType ActionType
        {
            get { return UserActionType.CreatedPuzzle; }
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Creator; }
        }

        protected override bool ShouldAwardBadge(User user, UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return true;
        }

        protected override User GetAffectedUser(UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override BadgeAwardInfo GetBadgeAwardInfo(UserAction action)
        {
            return new BadgeAwardInfo
            {
                Data = action.PuzzleId,
                DateAwarded = action.DateCreated
            };
        }
    }
}
