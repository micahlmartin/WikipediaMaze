using System;
using System.Collections.Generic;
using MongoDB.Driver.Builders;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Data.Mongo;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardCriticBadge : BaseBadgeAwarder
    {
        protected override Core.UserActionType ActionType
        {
            get { return Core.UserActionType.Voted; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Critic; }
        }

        protected override Core.User GetAffectedUser(Core.UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool ShouldAwardBadge(User user, UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return action.VoteType == VoteType.Down;
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override BadgeAwardInfo GetBadgeAwardInfo(UserAction action)
        {
            return new BadgeAwardInfo
            {
                DateAwarded = action.DateCreated,
                Data = action.PuzzleId
            };
        }
    }
}
