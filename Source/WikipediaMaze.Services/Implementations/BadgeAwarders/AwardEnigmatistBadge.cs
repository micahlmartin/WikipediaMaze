using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardEnigmatistBadge : BaseBadgeAwarder
    {
        private Puzzle _puzzle;

        protected override Core.UserActionType ActionType
        {
            get { return Core.UserActionType.Voted; }
        }

        protected override Core.BadgeType BadgeType
        {
            get { return Core.BadgeType.Enigmatist; }
        }

        protected override Core.User GetAffectedUser(Core.UserAction action)
        {
            _puzzle = Repository.All<Puzzle>().ById(action.PuzzleId.Value);
            var userId = _puzzle.CreatedById;
            return Repository.All<User>().ById(userId);
        }

        protected override bool AllowMultiple
        {
            get { return true; }
        }

        protected override bool ShouldAwardBadge(User user, UserAction action)
        {
            return _puzzle.VoteCount >= 50;
        }
    }
}
