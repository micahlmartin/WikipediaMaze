using System;
using System.Linq;
using StructureMap;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Services.Interfaces;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public abstract class BaseBadgeAwarder : IAwardBadge
    {
        public void AwardBadge(UserAction action)
        {
            if (ActionType != UserActionType.None && ActionType != action.Action)
                return;

            var affectedUser = GetAffectedUser(action);
            var badgeInfo = affectedUser.Badges.FirstOrDefault(x => x.Name == BadgeType.ToString());

            if (!AllowMultiple && badgeInfo != null && badgeInfo.Count > 0)
                return;

            if (!ShouldAwardBadge(affectedUser, action))
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
                affectedUser.Badges.Add(badgeInfo);
            }

            badgeInfo.Count++;

            affectedUser.Notifications.Add(new Notification
            {
                Id = Guid.NewGuid(),
                Message = "You have earned the " + BadgeType + "badge",
                Type = NotificationType.Badge
            });

            _repository.Save(affectedUser);
        }

        protected abstract UserActionType ActionType { get; }
        protected abstract BadgeType BadgeType { get; }
        protected abstract bool AllowMultiple { get; }
        protected abstract User GetAffectedUser(UserAction action);
        protected abstract bool ShouldAwardBadge(User user, UserAction action);

        private IRepository _repository;
        public IRepository Repository
        {
            get
            {
                if (_repository == null)
                    _repository = ObjectFactory.GetInstance<IRepository>();

                return _repository;
            }
            set
            {
                _repository = value;
            }
        }
    }
}
