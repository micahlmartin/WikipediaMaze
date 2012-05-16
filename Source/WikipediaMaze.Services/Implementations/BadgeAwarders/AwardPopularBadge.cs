using System;
using System.Collections.Generic;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using System.Linq;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardPopularBadge : BaseBadgeAwarder
    {
        private Puzzle _puzzle;

        protected override UserActionType ActionType
        {
            get { return UserActionType.SolvedPuzzle; }
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Popular ; }
        }

        protected override bool ShouldAwardBadge(Core.User user, Core.UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            return _puzzle.SolutionCount >= 10 && awardInfo.All(x => (int)x.Data != action.PuzzleId);
        }

        protected override User GetAffectedUser(Core.UserAction action)
        {
            _puzzle = Repository.All<Puzzle>().ById(action.PuzzleId.Value);
            return Repository.All<User>().ById(_puzzle.CreatedById);
        }

        protected override bool AllowMultiple
        {
            get { return true; }
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
