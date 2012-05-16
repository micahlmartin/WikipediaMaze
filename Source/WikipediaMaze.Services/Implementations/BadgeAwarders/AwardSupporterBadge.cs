using System;
using System.Collections.Generic;
using MongoDB.Driver.Builders;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Data.Mongo;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardSupporterBadge : BaseBadgeAwarder
    {
        protected override Core.UserActionType ActionType
        {
            get { return Core.UserActionType.Voted; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Supporter; }
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override Core.User GetAffectedUser(Core.UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool ShouldAwardBadge(Core.User user, Core.UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return action.VoteType == VoteType.Up;
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
