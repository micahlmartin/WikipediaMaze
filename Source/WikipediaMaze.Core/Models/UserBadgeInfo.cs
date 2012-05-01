using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.Core
{
    public class UserBadgeInfo
    {
        public string Name { get; set; }
        public BadgeLevel Level { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
    }
}
