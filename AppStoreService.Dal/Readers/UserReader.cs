using AppStoreService.Core;
using AppStoreService.Core.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

namespace AppStoreService.Dal.Readers
{
    using Query = Builders<User>;

    public class UserReader : BaseRepository<User>, IRead<User>
    {
        public UserReader(IMongoDatabase db) : base(db) { }

        public IEnumerable<User> Read()
        {
            return Collection.Find(Query.Filter.Empty).ToEnumerable();
        }
    }
}