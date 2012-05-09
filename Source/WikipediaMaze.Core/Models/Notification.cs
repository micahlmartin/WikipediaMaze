using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class Notification
    {
        public virtual Guid Id { get; set; }
        public virtual int NotificationId { get; set; }
        public virtual int UserId { get; set; }
        public virtual string Message { get; set; }
    }
}
