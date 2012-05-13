using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Data.NHibernate;

namespace DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var migrator = new MigrateData(new NHibernateRepository(), new WikipediaMaze.Data.Mongo.MongoRepository());
            migrator.Run();
        }
    }
}
