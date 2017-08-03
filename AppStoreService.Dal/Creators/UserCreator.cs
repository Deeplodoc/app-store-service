using AppStoreService.Core;
using AppStoreService.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Creators
{
    public class UserCreator : BaseRepository<User>, ICreate<User>
    {
        public UserCreator(IMongoDatabase db) : base(db, "users") { }

        public User Create(User item)
        {
            item.Id = ObjectId.GenerateNewId();
            Collection.InsertOne(item);
            item.ConfirmCode = Guid.NewGuid();
            return item;
        }

        public async Task<User> CreateAsync(User item)
        {
            item.Id = ObjectId.GenerateNewId();
            await Collection.InsertOneAsync(item);
            item.ConfirmCode = Guid.NewGuid();
            return item;
        }
    }
}