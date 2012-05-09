using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public class NotificationMap : ClassMap<Notification>
    {
        public NotificationMap()
        {
            Table("Notifications");
            Id(x => x.NotificationId, "Id");
            Map(x => x.Message);
            Map(x => x.UserId);
        }
    }
}
