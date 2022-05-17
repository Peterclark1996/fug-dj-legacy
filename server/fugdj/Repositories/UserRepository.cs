using fugdj.Dtos.Db;
using fugdj.Integration;
using MongoDB.Bson;
using MongoDB.Driver;
using static fugdj.Extensions.StringExtensions;

namespace fugdj.Repositories;

public interface IUserRepository
{
    public UserDbDto? GetUser(string userId);
    public void AddMediaForUser(string userId, MediaWithTagsDbDto mediaToAdd);
    public void CreateTagForMedia(string userId, MediaUpdateDbDto mediaUpdate, TagDbDto tagToAdd);
    public void UpdateMediaForUser(string userId, MediaUpdateDbDto mediaToUpdate);
    public void UpdateUser(string userId, UserUpdateDbDto userUpdate);
    public void DeleteMediaForUser(string userId, string hashCode);
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
        var update = Builders<UserDbDto>.Update.Push(u => u.MediaList, mediaToAdd);
        try
        {
            collection.FindOneAndUpdate(filter, update);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InternalServerException();
        }
    }
    
    public void CreateTagForMedia(string userId, MediaUpdateDbDto mediaToUpdate, TagDbDto tagToAdd)
    {
        var collection = _dataSourceClient.GetCollection<UserDbDto>(UserCollectionName);

        var encodedUserId = EncodeToBase64(userId);
        var filter = Builders<UserDbDto>.Filter.Eq("_id", encodedUserId);
        var update = Builders<UserDbDto>.Update
            .Set("MediaList.$[mediaWithTags].TagIds", mediaToUpdate.TagIds)
            .Push(u => u.TagList, tagToAdd);
        var mediaFilter = new[]
        {
            new BsonDocumentArrayFilterDefinition<BsonDocument>(
                new BsonDocument("mediaWithTags.Media.HashCode",
                    new BsonDocument("$eq", new BsonString(mediaToUpdate.HashCode))))
        };
        try
        {
            collection.UpdateOne(filter, update, new UpdateOptions {ArrayFilters = mediaFilter});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InternalServerException();
        }
    }

    public void UpdateUser(string userId, UserUpdateDbDto userUpdate)
    {
        var collection = _dataSourceClient.GetCollection<UserDbDto>(UserCollectionName);

        var encodedUserId = EncodeToBase64(userId);
        var filter = Builders<UserDbDto>.Filter.Eq("_id", encodedUserId);
        var update = Builders<UserDbDto>.Update.Set(u => u.Name, userUpdate.Name);
        try
        {
            collection.FindOneAndUpdate(filter, update);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InternalServerException();
        }
    }

    public void UpdateMediaForUser(string userId, MediaUpdateDbDto mediaToUpdate)
    {
        var collection = _dataSourceClient.GetCollection<UserDbDto>(UserCollectionName);

        var encodedUserId = EncodeToBase64(userId);
        var filter = Builders<UserDbDto>.Filter.Eq("_id", encodedUserId);
        var update = Builders<UserDbDto>.Update
            .Set("MediaList.$[mediaWithTags].TagIds", mediaToUpdate.TagIds)
            .Set("MediaList.$[mediaWithTags].Media.Name", mediaToUpdate.Name);
        var mediaFilter = new[]
        {
            new BsonDocumentArrayFilterDefinition<BsonDocument>(
                new BsonDocument("mediaWithTags.Media.HashCode",
                    new BsonDocument("$eq", new BsonString(mediaToUpdate.HashCode))))
        };
        try
        {
            collection.UpdateOne(filter, update, new UpdateOptions {ArrayFilters = mediaFilter});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InternalServerException();
        }
    }

    public void DeleteMediaForUser(string userId, string hashCode)
    {
        var collection = _dataSourceClient.GetCollection<UserDbDto>(UserCollectionName);

        var encodedUserId = EncodeToBase64(userId);
        var userFilter = Builders<UserDbDto>.Filter.Eq("_id", encodedUserId);
        var mediaFilter = Builders<MediaWithTagsDbDto>.Filter.Where(m => m.Media.HashCode == hashCode);
        var delete = Builders<UserDbDto>.Update.PullFilter(u => u.MediaList, mediaFilter);
        try
        {
            collection.FindOneAndUpdate(userFilter, delete);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InternalServerException();
        }
    }
}