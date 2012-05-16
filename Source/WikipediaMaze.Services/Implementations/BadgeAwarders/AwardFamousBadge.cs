﻿using System;
using System.Collections.Generic;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using System.Linq;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardFamousBadge : BaseBadgeAwarder
    {
        private Puzzle _puzzle;

        protected override UserActionType ActionType
        {
            get { return UserActionType.SolvedPuzzle; }
        }

        protected override BadgeType BadgeType
        {
            get { return Core.BadgeType.Famous; }
        }

        protected override bool AllowMultiple
        {
            get { return true; }
        }

        protected override User GetAffectedUser(UserAction action)
        {
            _puzzle = Repository.All<Puzzle>().ById(action.PuzzleId.Value);
            var userId = _puzzle.CreatedById;
            return Repository.All<User>().ById(userId);
        }

        protected override bool ShouldAwardBadge(User user, UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return _puzzle.SolutionCount >= 50 && awardInfo.All(x => (int)x.Data != action.PuzzleId);
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
