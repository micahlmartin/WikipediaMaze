using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WikipediaMaze.Core.Properties;
using StructureMap;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services
{
    public class PuzzleLeaderUpdateService : RecurringServiceBase
    {
        public override string ServiceName
        {
            get { return "PuzzleLeaderUpdateService"; }
        }

        public override int Interval
        {
            get
            {
                return Settings.Default.PuzzleLeaderUpdateInterval;
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
                    command.CommandText = "EXEC UpdatePuzzleLeadCount";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
