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
                return Settings.PuzzleLeaderUpdateInterval;
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
                    command.CommandText = "EXEC UpdatePuzzleLeadCount";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
