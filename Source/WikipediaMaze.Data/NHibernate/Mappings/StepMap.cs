using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.NHibernate.Mappings
{
    public sealed class StepMap : ClassMap<Step>
    {
        public StepMap()
        {
            Table("Steps");
            Id(s => s.Id);
            Map(s => s.Topic);
            Map(s => s.StepNumber);
            Map(x => x.SolutionId);
        }
    }
}
