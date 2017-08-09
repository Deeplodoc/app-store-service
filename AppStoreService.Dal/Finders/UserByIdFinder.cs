using AppStoreService.Core;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Finders
{
    using Query = Builders<User>;

    public class UserByIdFinder : BaseRepository<User>, IFilter<UserByIdModel, User>
    {
        public UserByIdFinder(IMongoDatabase db) : base(db, "users") { }

        public User Filter(UserByIdModel model)
        {
            FilterDefinition<User> filter = ById(model);
            if (filter != null)
            {
                return Collection.Find(filter).SingleOrDefault();
            }

            return null;
        }

        public Task<User> FilterAsync(UserByIdModel model)
        {
            FilterDefinition<User> filter = ById(model);
            if (filter != null)
            {
                return Collection.Find(filter).SingleOrDefaultAsync();
            }

            return null;
        }

        private FilterDefinition<User> ById(UserByIdModel model)
        {
            if (!string.IsNullOrEmpty(model.UserId) && ObjectId.TryParse(model.UserId, out ObjectId userId))
            {
                return Query.Filter.Eq(u => u.Id, userId);
            }

            return null;
        }
    }
}