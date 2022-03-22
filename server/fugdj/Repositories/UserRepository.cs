using fugdj.Dtos.Db;
using fugdj.Integration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using static fugdj.Extensions.StringExtensions;

namespace fugdj.Repositories;

public interface IUserRepository
{
    public UserDbDto? GetUser(string userId);
}

public class UserRepository : IUserRepository
{
    private const string UserCollectionName = "user-data";
        
    private readonly IDataSourceClient _dataSourceClient;

    public UserRepository(IDataSourceClient dataSourceClient)
    {
        _dataSourceClient = dataSourceClient;
    }
        
    public UserDbDto? GetUser(string userId)
    {
        var collection = _dataSourceClient.GetCollection(UserCollectionName);

        var encodedUserId = EncodeToBase64(userId);
        var filter = Builders<BsonDocument>.Filter.Eq("_id", encodedUserId);
        var item = collection.Find(filter).FirstOrDefault();
        if (item == null) return null;
        
        var userData = BsonSerializer.Deserialize<UserDbDto>(item);

        return userData;
    }
}