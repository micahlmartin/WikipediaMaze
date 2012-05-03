using System;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace WikipediaMaze.Data.Mongo
{
    public class LongIdGenerator : IIdGenerator
    {
        public object GenerateId(object container, object document)
        {
            var collection = (MongoCollection) container;
            var result = MongoRepository.Database.GetCollection(MongoRepository.SequenceCollectionName).FindAndModify(Query.EQ("_id", collection.Name), SortBy.Null, new UpdateBuilder().Inc("seq", 1L), true, true);
            return result.ModifiedDocument["seq"].AsInt64;
        }

        public bool IsEmpty(object id)
        {
            return id.Equals(0L);
        }
    }
}