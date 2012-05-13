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

        protected override bool ShouldAwardBadge(Core.User user, Core.UserAction action)
        {
            return MongoRepository.Database.GetCollection(MongoRepository.GetCollectionNamingConvention(typeof(Puzzle))).Count(Query.ElemMatch("Votes", Query.And(Query.EQ("UserId", user.Id), Query.EQ("VoteType", VoteType.Down)))) > 0;
        }

        protected override bool AllowMultiple
        {
            get { return false; }
        }
    }
}
