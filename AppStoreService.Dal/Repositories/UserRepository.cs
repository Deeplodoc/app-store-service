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
            item.Id = ObjectId.GenerateNewId();
            _collection.InsertOne(item);
            return item;
        }

        public void Delete(string itemIdent)
        {
            if (!string.IsNullOrEmpty(itemIdent) && ObjectId.TryParse(itemIdent, out ObjectId id))
                _collection.DeleteOne(ById(id));
        }

        public IEnumerable<User> Read()
        {
            return _collection.Find(Query.Filter.Empty).ToEnumerable();
        }

        public void Update(User item)
        {
            if (item.Id != null && ObjectId.TryParse(item.Id.ToString(), out ObjectId id))
                _collection.FindOneAndUpdate(ById(id), UpdateItem(item), IsUpsert(false));
        }

        private FilterDefinition<User> ById(object userId)
        {
            return Query.Filter.Eq(u => u.Id, userId);
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