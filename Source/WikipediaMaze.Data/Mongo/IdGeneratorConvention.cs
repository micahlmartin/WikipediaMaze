using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace WikipediaMaze.Data.Mongo
{
    public class IdGeneratorConvention : IIdGeneratorConvention
    {
        private readonly IIdGeneratorConvention _baseConvention = new LookupIdGeneratorConvention();

        public IIdGenerator GetIdGenerator(MemberInfo memberInfo)
        {
            
            if(memberInfo.MemberType != MemberTypes.Property)
                return _baseConvention.GetIdGenerator(memberInfo);

            var propInfo = (PropertyInfo) memberInfo;
            if (propInfo.PropertyType == typeof(int))
                return new IntIdGenerator();
            if (propInfo.PropertyType == typeof(long))
                return new LongIdGenerator();

            return _baseConvention.GetIdGenerator(memberInfo);
        }
    }
}
