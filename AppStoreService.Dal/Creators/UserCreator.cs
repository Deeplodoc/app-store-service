using AppStoreService.Core;
using AppStoreService.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AppStoreService.Dal.Creators
{
    public class UserCreator : BaseRepository<User>, ICreate<User>
    {
        public UserCreator(IMongoDatabase db) : base(db) { }

        public User Create(User item)
        {
            item.Id = ObjectId.GenerateNewId();
            Collection.InsertOne(item);
            return item;
        }
    }
}