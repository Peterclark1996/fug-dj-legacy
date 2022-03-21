using fugdj.Dtos.Db;
using fugdj.Integration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace fugdj.Repositories;

public interface IRoomRepository
{
    public IEnumerable<RoomDbDto> GetAllRooms();
    public RoomDbDto? GetRoomData(Guid roomId);
}

public class RoomRepository : IRoomRepository
{
    private const string RoomCollectionName = "room-data";
        
    private readonly IDataSourceClient _dataSourceClient;

    public RoomRepository(IDataSourceClient dataSourceClient)
    {
        _dataSourceClient = dataSourceClient;
    }
        
    public IEnumerable<RoomDbDto> GetAllRooms()
    {
        var collection = _dataSourceClient.GetCollection(RoomCollectionName);

        var rooms = collection
            .Find(Builders<BsonDocument>.Filter.Empty)
            .ToList()
            .Select(doc => BsonSerializer.Deserialize<RoomDbDto>(doc));

        return rooms ?? throw new NullReferenceException();
    }

    public RoomDbDto? GetRoomData(Guid roomId)
    {
        var collection = _dataSourceClient.GetCollection(RoomCollectionName);

        var filter = Builders<BsonDocument>.Filter.Eq("_id", roomId.ToString());
        var item = collection.Find(filter).FirstOrDefault();
        if (item == null) return null;
        
        var roomData = BsonSerializer.Deserialize<RoomDbDto>(item);

        return roomData;
    }
}