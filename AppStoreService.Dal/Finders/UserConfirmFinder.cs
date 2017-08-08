using AppStoreService.Core;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Finders
{
    using Query = Builders<User>;

    public class UserConfirmFinder : BaseRepository<User>, IFilter<UserConfirmModel, User>
    {
        public UserConfirmFinder(IMongoDatabase db) : base(db, "users") { }

        public User Filter(UserConfirmModel model)
        {
            FilterDefinition<User> filter = ByUserConfirm(model);
            if (filter != null)
            {
                return Collection.Find(filter).SingleOrDefault();
            }

            return null;
        }

        public async Task<User> FilterAsync(UserConfirmModel model)
        {
            FilterDefinition<User> filter = ByUserConfirm(model);
            if (filter != null)
            {
                return await Collection.Find(filter).SingleOrDefaultAsync();
            }

            return null;
        }

        private FilterDefinition<User> ById(object id)
        {
            return Query.Filter.Eq(u => u.Id, id);
        }

        private FilterDefinition<User> ByCodeConfirm(string code)
        {
            return Query.Filter.Eq(u => u.ConfirmCode, code);
        }

        private FilterDefinition<User> ByUserConfirm(UserConfirmModel model)
        {
            if (!string.IsNullOrEmpty(model.UserId) && ObjectId.TryParse(model.UserId, out ObjectId id) &&
                !string.IsNullOrEmpty(model.Code) && Guid.TryParse(model.Code, out Guid code))
            {
                return Query.Filter.And(ById(id), ByCodeConfirm(code.ToString()));
            }

            return null;
        }
    }
}