using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core.Properties;
using WikipediaMaze.Data;
using System.Data.SqlClient;

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
                return Settings.Default.UpdatePuzzleSolutionCountInterval;
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
                    command.CommandText = "EXEC UpdateSolutionCount";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
