using AppStoreService.Core;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Finders
{
    using Query = Builders<User>;

    public class UserLoginFinder : BaseRepository<User>, IFilter<UserLoginFindModel, User>
    {
        public UserLoginFinder(IMongoDatabase db) : base(db, "users") { }

        public User Filter(UserLoginFindModel model)
        {
            return Collection.Find(ByUserLogin(model)).SingleOrDefault();
        }

        public async Task<User> FilterAsync(UserLoginFindModel model)
        {
            return await Collection.Find(ByUserLogin(model)).SingleOrDefaultAsync();
        }

        private FilterDefinition<User> ByLogin(string login)
        {
            return Query.Filter.Eq(u => u.Login, login);
        }

        private FilterDefinition<User> ByPassword(string password)
        {
            return Query.Filter.Eq(u => u.Password, password);
        }

        private FilterDefinition<User> ByUserLogin(UserLoginFindModel model)
        {
            return Query.Filter.And(ByLogin(model.Login), ByPassword(model.Password));
        }
    }
}