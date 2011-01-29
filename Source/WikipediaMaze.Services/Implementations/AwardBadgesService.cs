﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using StructureMap;
using WikipediaMaze.Core.Properties;
using WikipediaMaze.Data;

namespace WikipediaMaze.Services
{
    public class AwardBadgesService : RecurringServiceBase
    {
        /// <summary>
        /// The unique name of the service.
        /// </summary>
        public override string ServiceName
        {
            get { return "Award Badge Service"; }
        }

        public override int Interval
        {
            get
            {
                return Settings.Default.AwardBadgeInterval;
            }
            set
            {
                base.Interval = value;
            }
        }

        /// <summary>
        /// Awards the badges
        /// </summary>
        protected override void DoWork()
        {
            using (var connection = new SqlConnection(Settings.Default.WikipediaMazeConnection))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC AwardBadges";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
