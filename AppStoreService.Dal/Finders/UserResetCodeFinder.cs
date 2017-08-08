using AppStoreService.Core;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Finders
{
    using Query = Builders<User>;

    public class UserResetCodeFinder : BaseRepository<User>, IFilter<UserResetCodeModel, User>
    {
        public UserResetCodeFinder(IMongoDatabase db) : base(db, "users") { }

        public User Filter(UserResetCodeModel model)
        {
            Guid? code = GetGuidCode(model.Code);
            if (code.HasValue)
            {
                return Collection.Find(ByResetCode(code.Value.ToString())).SingleOrDefault();
            }

            return null;
        }

        public async Task<User> FilterAsync(UserResetCodeModel model)
        {
            Guid? code = GetGuidCode(model.Code);
            if (code.HasValue)
            {
                return await Collection.Find(ByResetCode(code.Value.ToString())).SingleOrDefaultAsync();
            }

            return null;
        }

        private FilterDefinition<User> ByResetCode(string code)
        {
            return Query.Filter.Eq(u => u.ResetPasswordCode, code);
        }

        private Guid? GetGuidCode(string code)
        {
            if (Guid.TryParse(code, out Guid guidCode))
            {
                return guidCode;
            }

            return null;
        }
    }
}