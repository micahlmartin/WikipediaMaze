using System;
using System.Collections.Generic;
using System.Linq;

namespace WikipediaMaze.Core
{
    public class User : IValidate
    {
        private IList<UserBadgeInfo> _badges;
        private IList<OpenIdentifier> _openIdentifiers;
        private IList<Notification> _notifications;

        public virtual IList<UserBadgeInfo> Badges
        {
            get
            {
                if (_badges == null)
                    _badges = new List<UserBadgeInfo>();

                return _badges;
            }
            set
            {
                _badges = value;
            }
        }
        public virtual IList<OpenIdentifier> OpenIdentifiers
        {
            get
            {
                if (_openIdentifiers == null)
                    _openIdentifiers = new List<OpenIdentifier>();

                return _openIdentifiers;
            }
            set
            {
                _openIdentifiers = value;
            }
        }
        public virtual IList<Notification> Notifications
        {
            get
            {
                if (_notifications == null)
                    _notifications = new List<Notification>();

                return _notifications;
            }
            set
            {
                _notifications = value;
            }
        }

        public virtual int Id { get;  set; }
        public virtual DateTime LastVisit { get; set; }
        public virtual int Reputation { get; set; }
        public virtual string RealName { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual string Location { get; set; }
        public virtual string Email { get; set; }
        public virtual string Photo { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string PreferredUserName { get; set; }
        public virtual int LeadingPuzzleCount { get; set; }
        public virtual string TwitterUserName { get; set; }
        public virtual string GetGravatarUrl(int? size)
        {

            if (!string.IsNullOrEmpty(Email))
                return Email.AsGravatarUrl(size);

            return (PreferredUserName + "@wikipediamaze.com").AsGravatarUrl(size);
        }

        #region IValidate Members

        public virtual bool IsValid
        {
            get { return RuleViolations.Count() == 0; }
        }

        public virtual IEnumerable<RuleViolation> RuleViolations
        {
            get 
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    if (!RegularExpressions.EmailRegex.IsMatch(Email))
                        yield return new RuleViolation("Email address is invalid.", "Email");
                }

                if (!string.IsNullOrEmpty(RealName))
                {
                    if (!RegularExpressions.UserNameRegex.IsMatch(RealName))
                        yield return new RuleViolation("Name is invalid.", "RealName");
                }

                if (string.IsNullOrEmpty(DisplayName))
                    yield return new RuleViolation("Display Name cannot be blank.", "DisplayName");
                else if (!RegularExpressions.UserNameRegex.IsMatch(DisplayName))
                    yield return new RuleViolation("Display Name is invalid.", "DisplayName");
            }
        }

        #endregion

        public virtual int GoldBadgeCount
        {
            get { return Badges.Where(x => x.Level == BadgeLevel.Gold).Sum(x => x.Count); }
        }
        public virtual int SilverBadgeCount
        {
            get { return Badges.Where(x => x.Level == BadgeLevel.Silver).Sum(x => x.Count); }
        }
        public virtual int BronzeBadgeCount
        {
            get { return Badges.Where(x => x.Level == BadgeLevel.Bronze).Sum(x => x.Count); }
        }
    }
}