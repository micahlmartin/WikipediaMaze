﻿using System;
using System.Collections.Generic;
using System.Linq;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardCrazedBadge : BaseBadgeAwarder
    {
        private readonly List<int> _solvedPuzzleIds = new List<int>();

        protected override Core.UserActionType ActionType
        {
            get { return UserActionType.SolvedPuzzle; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Crazed; }
        }

        protected override bool ShouldAwardBadge(User user, UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            var numberOfDaysToCheck = 60;
            var now = DateTime.UtcNow;
            var actions = Repository.All<UserAction>().Where(x => x.UserId == user.Id && x.DateCreated > now.AddDays(numberOfDaysToCheck * -1) && x.Action == UserActionType.SolvedPuzzle).ToList();

            var awardBadge = true;
            var counter = 0;

            while (counter < numberOfDaysToCheck && awardBadge)
            {
                var dateToCheck = now.AddDays(counter * -1).Date;
                var matchingAction = actions.FirstOrDefault(x => x.DateCreated.Date == dateToCheck);

                if (matchingAction == null)
                    awardBadge = false;
                else
                    _solvedPuzzleIds.Add(matchingAction.PuzzleId.Value);

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

        protected override BadgeAwardInfo GetBadgeAwardInfo(UserAction action)
        {
            return new BadgeAwardInfo
            {
                DateAwarded = action.DateCreated,
                Data = _solvedPuzzleIds
            };
        }
    }
}
