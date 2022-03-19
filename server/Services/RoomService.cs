using fugdj.Dtos.Http;
using fugdj.Repositories;

namespace fugdj.Services;

public interface IRoomService
{
    public IEnumerable<RoomNameHttpDto> GetAllRooms();
    public RoomNameHttpDto GetRoomData(Guid roomId);
}

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public IEnumerable<RoomNameHttpDto> GetAllRooms() =>
        _roomRepository.GetAllRooms().Select(r => new RoomNameHttpDto(Guid.Parse(r.Id), r.Name));

    public RoomNameHttpDto GetRoomData(Guid roomId)
    {
        var room = _roomRepository.GetRoomData(roomId);
        return new RoomNameHttpDto(Guid.Parse(room.Id), room.Name);
    }
}