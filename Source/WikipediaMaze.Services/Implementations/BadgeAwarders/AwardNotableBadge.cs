using System;
using System.Collections.Generic;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using System.Linq;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardNotableBadge : BaseBadgeAwarder
    {
        private Puzzle _puzzle;

        protected override Core.UserActionType ActionType
        {
            get { return Core.UserActionType.SolvedPuzzle; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Notable; }
        }

        protected override bool AllowMultiple
        {
            get { return true; }
        }

        protected override Core.User GetAffectedUser(Core.UserAction action)
        {
            _puzzle = Repository.All<Puzzle>().ById(action.PuzzleId.Value);
            return Repository.All<User>().ById(_puzzle.CreatedById);
        }

        protected override bool ShouldAwardBadge(Core.User user, Core.UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return _puzzle.SolutionCount >= 25 && awardInfo.All(x => (int)x.Data != action.PuzzleId);
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