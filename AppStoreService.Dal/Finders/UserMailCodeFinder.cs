using AppStoreService.Core;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Finders
{
    using Query = Builders<User>;

    public class UserMailCodeFinder : BaseRepository<User>, IFilter<UserResetPassModel, User>
    {
        public UserMailCodeFinder(IMongoDatabase db) : base(db, "users") { }

        public User Filter(UserResetPassModel model)
        {
            return Collection.Find(ByResetModel(model)).SingleOrDefault();
        }

        public Task<User> FilterAsync(UserResetPassModel model)
        {
            return Collection.Find(ByResetModel(model)).SingleOrDefaultAsync();
        }

        private FilterDefinition<User> ByEmail(string email)
        {
            return Query.Filter.Eq(u => u.Email, email);
        }

        private FilterDefinition<User> ByCode(string code)
        {
            return Query.Filter.Eq(u => u.ResetPasswordCode, code);
        }

        private FilterDefinition<User> ByResetModel(UserResetPassModel model)
        {
            return Query.Filter.And(ByEmail(model.Email), ByCode(model.Code));
        }
    }
}