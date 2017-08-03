using AppStoreService.Core;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Finders
{
    using Query = Builders<User>;

    public class UserMailFinder : BaseRepository<User>, IFilter<UserMailModel, User>
    {
        public UserMailFinder(IMongoDatabase db) : base(db, "users") { }

        public User Filter(UserMailModel model)
        {
            return Collection.Find(ByEmail(model.Email)).SingleOrDefault();
        }

        public Task<User> FilterAsync(UserMailModel model)
        {
            return Collection.Find(ByEmail(model.Email)).SingleOrDefaultAsync();
        }
        private FilterDefinition<User> ByEmail(string email)
        {
            return Query.Filter.Eq(u => u.Email, email);
        }
    }
}