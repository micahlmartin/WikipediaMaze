using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Data;
using System.Data.SqlClient;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public class UpdatePuzzleSolutionCountService : RecurringServiceBase
    {
        public override string ServiceName
        {
            get { return "UpdatePuzzleSolutionCountService"; }
        }

        public override int Interval
        {
            get
            {
                return Settings.UpdatePuzzleSolutionCountInterval;
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
                    command.CommandText = "EXEC UpdateSolutionCount";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
