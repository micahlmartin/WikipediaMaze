using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Services.Interfaces;

namespace WikipediaMaze.Services.Implementations.BadgeAwarders
{
    public abstract class BaseBadgeAwarder : IAwardBadge
    {
        public bool AwardBadge(UserAction action)
        {
            if (ActionType != UserActionType.None && ActionType != action.Action)
                return false;

            var affectedUser = GetAffectedUser(action);
            var badgeInfo = affectedUser.Badges.FirstOrDefault(x => x.Name == BadgeType.ToString());

            if (!AllowMultiple && badgeInfo != null && badgeInfo.Count > 0)
                return false;

            var awardInfo = badgeInfo == null ? new List<BadgeAwardInfo>() : badgeInfo.AwardInfo;

            if (!ShouldAwardBadge(affectedUser, action, awardInfo))
                return false;

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
            badgeInfo.AwardInfo.Add(GetBadgeAwardInfo(action));

            affectedUser.Notifications.Add(new Notification
            {
                Id = Guid.NewGuid(),
                Message = "You have earned the " + BadgeType + "badge",
                Type = NotificationType.Badge
            });

            _repository.Save(affectedUser);

            return true;
        }

        protected abstract UserActionType ActionType { get; }
        protected abstract BadgeType BadgeType { get; }
        protected abstract bool AllowMultiple { get; }
        protected abstract User GetAffectedUser(UserAction action);
        protected abstract bool ShouldAwardBadge(User user, UserAction action, IList<BadgeAwardInfo> awardInfo);
        protected abstract BadgeAwardInfo GetBadgeAwardInfo(UserAction action);

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
