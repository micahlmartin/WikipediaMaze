using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.Core
{
    public class UserBadgeInfo
    {
        private IList<BadgeAwardInfo> _awardInfo;

        public string Name { get; set; }
        public BadgeLevel Level { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public IList<BadgeAwardInfo> AwardInfo
        {
            get
            {
                if (_awardInfo == null)
                    _awardInfo = new List<BadgeAwardInfo>();

                return _awardInfo;
            }
            set { _awardInfo = value; }
        }
    }

    public class BadgeAwardInfo
    {
        public DateTime DateAwarded { get; set; }
        public object Data { get; set; }
    }
}
