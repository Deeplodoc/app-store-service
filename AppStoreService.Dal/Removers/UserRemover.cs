using AppStoreService.Core;
using AppStoreService.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AppStoreService.Dal.Removers
{
    using Query = Builders<User>;

    public class UserRemover : BaseRepository<User>, IDelete<string>
    {
        public UserRemover(IMongoDatabase db) : base(db) { }

        public void Delete(string itemIdent)
        {
            if (!string.IsNullOrEmpty(itemIdent) && ObjectId.TryParse(itemIdent, out ObjectId id))
                Collection.DeleteOne(ById(id));
        }

        private FilterDefinition<User> ById(object userId)
        {
            return Query.Filter.Eq(u => u.Id, userId);
        }
    }
}