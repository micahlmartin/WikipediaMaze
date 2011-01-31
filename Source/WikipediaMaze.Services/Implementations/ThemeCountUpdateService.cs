using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using StructureMap;
using WikipediaMaze.Data;
using WikipediaMaze.Core;

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
                return Settings.ThemeCountUpdateInterval;
            }
            set
            {
                base.Interval = value;
            }
        }

        protected override void DoWork()
        {
            using (var connection = new SqlConnection(Settings.WikipediaMazeConnection))
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
