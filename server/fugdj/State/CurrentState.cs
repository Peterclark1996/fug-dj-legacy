using fugdj.Dtos.Db;

namespace fugdj.State;

public static class CurrentState
{
    private static readonly Dictionary<Guid, RoomState> RoomStates = new();
        
    public static RoomState GetCurrentRoomState(Guid roomId, Func<RoomDbDto> getRoomData)
    {
        if (RoomStates.ContainsKey(roomId)) return RoomStates[roomId];

        var roomEntity = getRoomData();
        RoomStates[roomId] = new RoomState(Guid.Parse(roomEntity.Id), roomEntity.Name);
        return RoomStates[roomId];
    }
}