using MongoDB.Driver;

namespace WHITE_20.MongoDB
{
    public class MongoDBConnection
    {
        public MongoClient MongoClient { get; }
        public IMongoDatabase MongoDatabase { get; }

        public MongoDBConnection(string connectionString, string databaseName)
        {
            MongoClient = new(connectionString);
            MongoDatabase = MongoClient.GetDatabase(databaseName);
        }
    }
}
