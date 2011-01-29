using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public class OpenIdentifierMap : ClassMap<OpenIdentifier>
    {
        public OpenIdentifierMap()
        {
            Table("OpenIdentifiers");
            Id(x => x.Identifier);
            Map(x => x.IsPrimary);
            Map(x => x.UserId);
        }
    }
}
