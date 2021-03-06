using System.Linq;
using WikipediaMaze.Core;
using WikipediaMaze.Services;
using System.Collections.Generic;

namespace WikipediaMaze.ViewModels
{
    public class UserInfoViewModel
    {
        private readonly User _user;
        public UserInfoViewModel(User user)
        {
            _user = user;
        }

        public string GetGravatarUrl(int size)
        {
            return _user.GetGravatarUrl(size);
        }
        public string UserName
        {
            get
            {
                return _user.DisplayName;
            }
        }
        public int UserId
        {
            get
            {
                return _user.Id;
            }
        }
        public int Reputation
        {
            get { return _user.Reputation; }
        }
        public int GoldBadgeCount
        {
            get { return _user.Badges.Where(x => x.Level == BadgeLevel.Gold).Count(); }
        }
        public int SilverBadgeCount
        {
            get { return _user.Badges.Where(x => x.Level == BadgeLevel.Silver).Count(); }
        }
        public int BronzeBadgeCount
        {
            get { return _user.Badges.Where(x => x.Level == BadgeLevel.Bronze).Count(); }
        }
        public int LeadingPuzzleCount
        {
            get
            {
                return _user.LeadingPuzzleCount;
            }
        }
    }
}