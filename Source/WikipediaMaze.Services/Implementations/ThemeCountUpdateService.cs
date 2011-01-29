using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using StructureMap;
using WikipediaMaze.Core.Properties;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services
{
    public class ThemeCountUpdateService : RecurringServiceBase
    {
        public override string ServiceName
        {
            get { return "Theme Count Update Service"; }
        }

        public override int Interval
        {
            get
            {
                return Settings.Default.ThemeCountUpdateInterval;
            }
            set
            {
                base.Interval = value;
            }
        }

        protected override void DoWork()
        {
            using (var connection = new SqlConnection(Settings.Default.WikipediaMazeConnection))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC UpdateThemeCount";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
