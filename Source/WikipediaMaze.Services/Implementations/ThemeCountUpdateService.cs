using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MongoDB.Driver.Builders;
using StructureMap;
using WikipediaMaze.Data;
using WikipediaMaze.Core;
using WikipediaMaze.Data.Mongo;
using MongoDB.Driver.Linq;

namespace WikipediaMaze.Services
{
    public class ThemeCountUpdateService : RecurringServiceBase
    {
        private readonly IRepository _repository;

        public ThemeCountUpdateService(IRepository repository)
        {
            _repository = repository;
        }

        public override string ServiceName
        {
            get { return "Theme Count Update Service"; }
        }

        public override int Interval
        {
            get
            {
                return Settings.ThemeCountUpdateInterval;
            }
            set
            {
                base.Interval = value;
            }
        }

        protected override void DoWork()
        {
            foreach (var theme in _repository.All<Theme>())
            {
                var currentTheme = theme;
                var count = _repository.All<Puzzle>().Where(x => x.Themes.Contains(currentTheme.Name)).Count();
                theme.Count = count;
                _repository.Save(theme);
            }
        }
    }
}
