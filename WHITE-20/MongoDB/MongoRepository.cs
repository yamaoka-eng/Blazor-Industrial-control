using MongoDB.Driver;

namespace WHITE_20.MongoDB
{
    public class MongoRepository<T> where T : class, IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(MongoDBConnection dbConnection, string collectionName)
        {
            _collection = dbConnection.MongoDatabase.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<T?> GetByIdAsync(string id) =>
            await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();

        public async Task CreateAsync(T entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task CreateManyAsync(IEnumerable<T> entities) =>
            await _collection.InsertManyAsync(entities);

        public async Task UpdateAsync(T entity) =>
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", entity.Id), entity);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));

        public async Task BulkUpdateAsync(IEnumerable<T> entities)
        {
            var bulkOps = entities.Select(entity =>
                new ReplaceOneModel<T>(Builders<T>.Filter.Eq("Id", entity.Id), entity) as WriteModel<T>
            ).ToList();

            if (bulkOps.Count > 0)
            {
                await _collection.BulkWriteAsync(bulkOps);
            }
        }

        public async Task BulkDeleteAsync(IEnumerable<T> entities)
        {
            var bulkOps = entities.Select(entity =>
                new DeleteOneModel<T>(Builders<T>.Filter.Eq("Id", entity.Id)) as WriteModel<T>
            ).ToList();

            if (bulkOps.Count > 0)
            {
                await _collection.BulkWriteAsync(bulkOps);
            }
        }
    }
}
