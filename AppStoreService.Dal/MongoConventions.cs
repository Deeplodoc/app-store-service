using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Linq;
using AppStoreService.Core;

namespace AppStoreService.Dal
{
    public class MongoConventions
    {
        public static void BindConventions()
        {
            var convention = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new StringObjectIdConvention(),
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("TeasersConventions", convention, type => true);
        }

        public class StringObjectIdConvention : ConventionBase, IClassMapConvention
        {
            public void Apply(BsonClassMap classMap)
            {
                if (classMap.ClassType.Namespace.Contains("AppStoreService.Core"))
                {
                    foreach (var map in classMap.DeclaredMemberMaps.Where(x => x != null))
                    {
                        if (map.MemberType == typeof(object) &&
                            (map.MemberName == "Id" || map.MemberName == "ObjectId"))
                        {
                            classMap.MapIdMember(map.MemberInfo);
                            map.SetIdGenerator(new ObjectIdGenerator());
                        }

                        if (map.MemberType.IsGenericParameter && map.MemberType.GetGenericTypeDefinition() ==
                            typeof(NullValueDictionary<,>))
                        {
                            classMap.MapExtraElementsMember(map.MemberInfo);
                        }
                    }
                }
            }
        }
    }
}