using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardPlayerBadge : BaseBadgeAwarder
    {
        protected override Core.UserActionType ActionType
        {
            get { return Core.UserActionType.SolvedPuzzle; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Player; }
        }

        protected override bool ShouldAwardBadge(Core.User user, Core.UserAction action, IList<BadgeAwardInfo> awardInfo)
        {
            var puzzleIds = Repository.All<Solution>().Where(x => x.UserId == action.UserId).Select(x => x.PuzzleId).Distinct().ToList();
            return Repository.All<Puzzle>().Any(x => x.Id.In(puzzleIds) && x.CreatedById != user.Id);
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
                Data = action.PuzzleId
            };
        }
    }
}
