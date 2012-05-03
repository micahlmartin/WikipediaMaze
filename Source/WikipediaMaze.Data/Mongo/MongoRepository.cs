using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NHibernate;
using MongoDB.Driver.Linq;
using WikipediaMaze.Core;

namespace WikipediaMaze.Data.Mongo
{
    public class MongoRepository : IRepository
    {
        public static readonly MongoServer Server;
        public static readonly MongoDatabase Database;
        public const string SequenceCollectionName = "Sequences";

        static MongoRepository()
        {
            var conventions = new ConventionProfile();
            conventions.SetIgnoreExtraElementsConvention(new AlwaysIgnoreExtraElementsConvention());
            conventions.SetIdMemberConvention(new NamedIdMemberConvention("Id", "Identifier", "Theme", "Name"));
            conventions.SetIgnoreIfDefaultConvention(new AlwaysIgnoreIfDefaultConvention());
            conventions.SetIdGeneratorConvention(new IdGeneratorConvention());

            BsonClassMap.RegisterConventions(conventions, (t) => true);

            var mongoConnectionString = ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString;
            Server = MongoServer.Create(mongoConnectionString);
            var mongoUrl = MongoUrl.Create(mongoConnectionString);
            Database = Server.GetDatabase(mongoUrl.DatabaseName);
        }

        //public MongoCollection<ActionItem> Actions
        //{
        //    get { return _db.GetCollection<ActionItem>("Actions"); }
        //}
        //public MongoCollection<Badge> Badges
        //{
        //    get { return _db.GetCollection<Badge>("Badges"); }
        //}
        //public MongoCollection<Notification> Notifications
        //{
        //    get { return _db.GetCollection<Notification>("Notifications"); }
        //}
        //public MongoCollection<OpenIdentifier> OpenIdentifiers
        //{
        //    get { return _db.GetCollection<OpenIdentifier>("OpenIdentifiers"); }
        //}
        ////public MongoCollection<PuzzleDetailView> PuzzleDetailView
        ////{
        ////    get { return _db.GetCollection<PuzzleDetailView>("PuzzleDetailView"); }
        ////}
        //public MongoCollection<Puzzle> Puzzles
        //{
        //    get { return _db.GetCollection<Puzzle>("Puzzles"); }
        //}
        //public MongoCollection<PuzzleTheme> PuzzleThemes
        //{
        //    get { return _db.GetCollection<PuzzleTheme>("PuzzleThemes"); }
        //}
        //public MongoCollection<Solution> Solutions
        //{
        //    get { return _db.GetCollection<Solution>("Solutions"); }
        //}
        ////public MongoCollection<SolutionProfile> SolutionProfileView
        ////{
        ////    get { return _db.GetCollection<SolutionProfile>("SolutionProfileView"); }
        ////}
        //public MongoCollection<Step> Steps
        //{
        //    get { return _db.GetCollection<Step>("Steps"); }
        //}
        //public MongoCollection<Theme> Themes
        //{
        //    get { return _db.GetCollection<Theme>("Themes"); }
        //}
        //public MongoCollection<User> Users
        //{
        //    get { return _db.GetCollection<User>("Users"); }
        //}
        //public MongoCollection<Vote> Votes
        //{
        //    get { return _db.GetCollection<Vote>("Votes"); }
        //}

        public ISession OpenSession()
        {
            throw new NotSupportedException();
        }

        public IQueryable<TModel> All<TModel>()
        {
            return Database.GetCollection<TModel>(GetCollectionNamingConvention(typeof(TModel))).AsQueryable();
        }

        public void Save<TModel>(TModel model)
        {
            Database.GetCollection<TModel>(GetCollectionNamingConvention(typeof(TModel))).Save(model);
        }

        public void InsertBatch<TModel>(IEnumerable<TModel> model)
        {
            Database.GetCollection<TModel>(GetCollectionNamingConvention(typeof(TModel))).InsertBatch(model);
        }

        public void Update<TModel>(TModel model)
        {
            throw new NotSupportedException("Call Save method instead");
        }

        public void Delete<TModel>(TModel model)
        {
            var t = typeof (TModel);
            var idMemberName = BsonClassMap.LookupClassMap(t).IdMemberMap.MemberName;
            var val = t.GetProperty(idMemberName).GetValue(model, null);

            Database.GetCollection<TModel>(GetCollectionNamingConvention(typeof(TModel))).Remove(Query.EQ("_id", MongoDB.Bson.BsonTypeMapper.MapToBsonValue(val)));
        }

        public void Delete<TModel>(object id)
        {
            Database.GetCollection<TModel>(GetCollectionNamingConvention(typeof(TModel))).Remove(Query.EQ("_id", MongoDB.Bson.BsonTypeMapper.MapToBsonValue(id)));
        }

        public ITransaction BeginTransaction()
        {
            throw new NotSupportedException();
        }

        public void Flush()
        {
            throw new NotSupportedException();
        }

        public static Func<Type, string> GetCollectionNamingConvention = (t) => t.Name;
    }
}
