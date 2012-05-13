using WikipediaMaze.Core;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public class AwardCreatorBadge : BaseBadgeAwarder
    {
        protected override UserActionType ActionType
        {
            get { return UserActionType.CreatedPuzzle; }
        }

        protected override BadgeType BadgeType
        {
            get { return BadgeType.Creator; }
        }

        protected override bool ShouldAwardBadge(User user, UserAction action)
        {
            return true;
        }

        protected override User GetAffectedUser(UserAction action)
        {
            return Repository.All<User>().ById(action.UserId);
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }
    }
}
