using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class OpenIdentifier
    {
        public virtual string Identifier { get; set; }
        public virtual int UserId { get; set; }
        public virtual bool IsPrimary { get; set; }
    }
}
