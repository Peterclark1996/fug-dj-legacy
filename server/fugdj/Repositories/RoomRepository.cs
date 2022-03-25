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
        var collection = _dataSourceClient.GetCollection<RoomDbDto>(RoomCollectionName);

        var rooms = collection
            .Find(Builders<RoomDbDto>.Filter.Empty)
            .ToList();

        return rooms ?? throw new NullReferenceException();
    }

    public RoomDbDto? GetRoomData(Guid roomId)
    {
        var collection = _dataSourceClient.GetCollection<RoomDbDto>(RoomCollectionName);

        var filter = Builders<RoomDbDto>.Filter.Eq("_id", roomId.ToString());
        var roomData = collection.Find(filter).FirstOrDefault();
        return roomData;
    }
}