using System;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardBetaBadge : BaseBadgeAwarder
    {
        private static readonly DateTime CutoffDate = new DateTime(2010, 3, 1, 0, 0, 0, DateTimeKind.Utc);

        protected override UserActionType ActionType
        {
            get { return UserActionType.None; }
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Beta; }
        }

        protected override Core.User GetAffectedUser(UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool ShouldAwardBadge(User user, UserAction action)
        {
            return user.DateCreated < CutoffDate;
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }
    }
}
