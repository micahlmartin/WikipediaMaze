using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Services.Interfaces;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public abstract class AwardBadgeBase : IAwardBadge
    {
        private readonly IRepository _repository;

        protected AwardBadgeBase(IRepository repository)
        {
            _repository = repository;
        }

        protected abstract bool AllowMultiple { get; }

        public void AwardBadge(int userId)
        {
            var user = _repository.All<User>().ById(userId);
            var badgeInfo = user.Badges.FirstOrDefault(x => x.Name == BadgeType.ToString());

            if (!AllowMultiple && badgeInfo != null)
                return;

            if (!ShouldAwardBadge(user))
                return;

            if (badgeInfo == null)
            {
                badgeInfo = new UserBadgeInfo
                                        {
                                            Description = Badges.Beta.Description,
                                            Level = Badges.Beta.Level,
                                            Name = Badges.Beta.Name
                                        };
                user.Badges.Add(badgeInfo);
            }

            badgeInfo.Count++;
            user.Notifications.Add(new Notification
                                       {
                                           Id = Guid.NewGuid(),
                                           Message = "You have earned the " + BadgeType + "badge"
                                       });

            _repository.Save(user);
        }

        protected abstract bool ShouldAwardBadge(User user);
        protected abstract BadgeType BadgeType { get; }
        protected IRepository Repository
        {
            get { return _repository; }
        }
    }
}
