using MongoDB.Bson;
using MongoDB.Driver;

namespace fugdj.Integration;

public interface IDataSourceClient
{
    public IMongoCollection<BsonDocument> GetCollection(string collectionName);
}

public class DataSourceClient : IDataSourceClient
{
    private readonly MongoClient _dbClient;

    public DataSourceClient(IConfiguration configuration)
    {
        _dbClient = new MongoClient(configuration.GetConnectionString("MongoDb"));
    }

    public IMongoCollection<BsonDocument> GetCollection(string collectionName) =>
        _dbClient.GetDatabase(collectionName).GetCollection<BsonDocument>(collectionName);
}