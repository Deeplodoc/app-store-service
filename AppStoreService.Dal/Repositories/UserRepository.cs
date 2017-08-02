using System.Collections.Generic;
using AppStoreService.Core;
using AppStoreService.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AppStoreService.Dal.Repositories
{
    using Query = Builders<User>;

    public class UserRepository : ICreate<User>, IRead<User>, IUpdate<User>, IDelete<string>
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<User>("users");
        }

        public User Create(User item)
        {
            return _collection.FindOneAndUpdate(ById(item.Id.ToString()), UpdateItem(item), IsUpsert(true));
        }

        public void Delete(string itemIdent)
        {
            _collection.DeleteOne(ById(itemIdent));
        }

        public IEnumerable<User> Read()
        {
            return _collection.Find(Query.Filter.Empty).ToEnumerable();
        }

        public void Update(User item)
        {
            _collection.FindOneAndUpdate(ById(item.Id.ToString()), UpdateItem(item), IsUpsert(true));
        }

        private FilterDefinition<User> ById(string userId)
        {
            return Query.Filter.Eq(u => u.Id, new ObjectId(userId));
        }

        public UpdateDefinition<User> UpdateItem(User item)
        {
            return Query.Update
                .Set(u => u.Address, item.Address)
                .Set(u => u.BDay, item.BDay)
                .Set(u => u.Email, item.Email)
                .Set(u => u.FirstName, item.FirstName)
                .Set(u => u.IsConfirm, item.IsConfirm)
                .Set(u => u.LastName, item.LastName)
                .Set(u => u.Phone, item.Phone);
        }

        private FindOneAndUpdateOptions<User> IsUpsert(bool upsert)
        {
            return new FindOneAndUpdateOptions<User>
            {
                IsUpsert = upsert,
                ReturnDocument = ReturnDocument.After
            };
        }
    }
}