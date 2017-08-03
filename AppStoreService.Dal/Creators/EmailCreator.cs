using AppStoreService.Core;
using AppStoreService.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Creators
{
    public class EmailCreator : BaseRepository<Email>, ICreate<Email>
    {
        public EmailCreator(IMongoDatabase db) : base(db) { }

        public Email Create(Email item)
        {
            item.Id = ObjectId.GenerateNewId();
            Collection.InsertOne(item);
            return item;
        }

        public async Task<Email> CreateAsync(Email item)
        {
            item.Id = ObjectId.GenerateNewId();
            await Collection.InsertOneAsync(item);
            return item;
        }
    }
}