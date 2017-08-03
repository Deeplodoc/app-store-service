﻿using AppStoreService.Core;
using AppStoreService.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AppStoreService.Dal.Updaters
{
    using Query = Builders<User>;

    public class UserUpdater : BaseRepository<User>, IUpdate<User>
    {
        public UserUpdater(IMongoDatabase db) : base(db, "users") { }

        public void Update(User item)
        {
            if (item.Id != null && ObjectId.TryParse(item.Id.ToString(), out ObjectId id))
                Collection.FindOneAndUpdate(ById(id), UpdateItem(item), IsUpsert(false));
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