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

        public virtual void AwardBadge(int userId)
        {
            var user = _repository.All<User>().ById(userId);
            var badgeInfo = user.Badges.FirstOrDefault(x => x.Name == BadgeType.ToString());

            if (!AllowMultiple && badgeInfo != null)
                return;

            if (!ShouldAwardBadge(user))
                return;

            if (badgeInfo == null)
            {
                var badge = Badges.GetBadgeByType(BadgeType);

                badgeInfo = new UserBadgeInfo
                                        {
                                            Description = badge.Description,
                                            Level = badge.Level,
                                            Name = badge.Name
                                        };
                user.Badges.Add(badgeInfo);
            }

            AssignBadgeCount(badgeInfo);

            user.Notifications.Add(new Notification
                                       {
                                           Id = Guid.NewGuid(),
                                           Message = "You have earned the " + BadgeType + "badge"
                                       });

            _repository.Save(user);
        }

        protected abstract bool ShouldAwardBadge(User user);
        protected abstract BadgeType BadgeType { get; }
        protected virtual void AssignBadgeCount(UserBadgeInfo badgeInfo)
        {
            badgeInfo.Count++;
        }
        protected IRepository Repository
        {
            get { return _repository; }
        }
    }
}
