using MongoDB.Driver;

namespace AppStoreService.Dal
{
    public class BaseRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public BaseRepository(IMongoDatabase db, string collectionName)
        {
            _collection = db.GetCollection<T>(collectionName);
        }

        public virtual IMongoCollection<T> Collection
        {
            get
            {
                return _collection;
            }
        }
    }
}