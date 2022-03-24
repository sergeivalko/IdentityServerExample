using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Profile.Infrastructure.Database
{
    public static class MongoDbConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Domain.Profile>(map =>
            {
                map.AutoMap();
            });
        }
    }
}
