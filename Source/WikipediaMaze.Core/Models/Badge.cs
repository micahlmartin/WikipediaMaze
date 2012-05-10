using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class Badge
    {
        public virtual int Id { get; set; }
        public virtual BadgeLevel Level { get; set; }
        public virtual string Description { get; set; }
        public virtual string Name { get; set; }
    }
}
