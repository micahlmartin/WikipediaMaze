using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.Core
{
    public class UserBadge
    {
        public virtual int Id { get; set; }
        public virtual int BadgeId { get; set; }
        public virtual int UserId { get; set; }
        public virtual Badge Badge { get; set; }
    }
}
