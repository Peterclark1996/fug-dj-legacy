using fugdj.Dtos.Db;
using fugdj.Integration;
using MongoDB.Driver;
using static fugdj.Extensions.StringExtensions;

namespace fugdj.Repositories;

public interface IUserRepository
{
    public UserDbDto? GetUser(string userId);
    public void AddMediaForUser(string userId, MediaWithTagsDbDto mediaToAdd);
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
        var collection = _dataSourceClient.GetCollection<UserDbDto>(UserCollectionName);

        var encodedUserId = EncodeToBase64(userId);
        var filter = Builders<UserDbDto>.Filter.Eq("_id", encodedUserId);
        var userData = collection.Find(filter).FirstOrDefault();
        
        return userData;
    }
    
    public void AddMediaForUser(string userId, MediaWithTagsDbDto mediaToAdd)
    {
        var collection = _dataSourceClient.GetCollection<UserDbDto>(UserCollectionName);

        var encodedUserId = EncodeToBase64(userId);
        var filter = Builders<UserDbDto>.Filter.Eq("_id", encodedUserId);
        var update = Builders<UserDbDto>.Update.Push(e => e.MediaList, mediaToAdd);
        collection.FindOneAndUpdateAsync(filter, update);
    }
}