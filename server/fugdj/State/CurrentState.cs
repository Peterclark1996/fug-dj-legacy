using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Dtos.Db;

namespace fugdj.State
{
    public static class CurrentState
    {
        private static readonly Dictionary<Guid, RoomState> RoomStates = new();

        public static List<RoomState> GetAllActiveRoomStates() => RoomStates.Values.ToList();

        public static RoomState? GetRoomWithConnectedUser(string userId) =>
            RoomStates.Values.FirstOrDefault(r => r.GetUsers().Any(u => u.Id == userId));
        
        public static RoomState GetCurrentRoomState(Guid roomId, Func<RoomDbDto> getRoomData)
        {
            if (RoomStates.ContainsKey(roomId)) return RoomStates[roomId];

            var roomEntity = getRoomData();
            RoomStates[roomId] = new RoomState(Guid.Parse(roomEntity.Id), roomEntity.Name);
            return RoomStates[roomId];
        }
    }
}