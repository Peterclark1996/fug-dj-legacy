using fugdj.Dtos.Db;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace fugdj.Repositories;

public interface IRoomRepository
{
    public IEnumerable<RoomDbDto> GetAllRooms();
    public RoomDbDto GetRoomData(Guid roomId);
}

public class RoomRepository : IRoomRepository
{
    private const string RoomCollectionName = "room-data";
        
    private readonly IConfiguration _configuration;

    public RoomRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
        
    public IEnumerable<RoomDbDto> GetAllRooms()
    {
        var dbClient = new MongoClient(_configuration.GetConnectionString("MongoDb"));
        var database = dbClient.GetDatabase(RoomCollectionName);
        var collection = database.GetCollection<BsonDocument>(RoomCollectionName);

        var rooms = collection
            .Find(Builders<BsonDocument>.Filter.Empty)
            .ToList()
            .Select(doc => BsonSerializer.Deserialize<RoomDbDto>(doc));

        return rooms ?? throw new NullReferenceException();
    }

    public RoomDbDto GetRoomData(Guid roomId)
    {
        var dbClient = new MongoClient(_configuration.GetConnectionString("MongoDb"));
        var database = dbClient.GetDatabase(RoomCollectionName);
        var collection = database.GetCollection<BsonDocument>(RoomCollectionName);

        var filter = Builders<BsonDocument>.Filter.Eq("_id", roomId.ToString());
        var roomData = BsonSerializer.Deserialize<RoomDbDto>(collection.Find(filter).FirstOrDefault());
            
        return roomData ?? throw new NullReferenceException("Room with given id does not exist");
    }
}