﻿using System;
using WikipediaMaze.Data;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardYearlingBadge : BaseBadgeAwarder
    {
        protected override Core.UserActionType ActionType
        {
            get { return Core.UserActionType.None; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Yearling; }
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }

        protected override Core.User GetAffectedUser(Core.UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool ShouldAwardBadge(Core.User user, Core.UserAction action)
        {
            return DateTime.UtcNow.Date >= user.DateCreated.Date.AddYears(1);
        }
    }
}
