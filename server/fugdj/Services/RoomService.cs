using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Dtos.Http;
using fugdj.Repositories;
using fugdj.State;

namespace fugdj.Services
{
    public interface IRoomService
    {
        public IEnumerable<RoomNameHttpDto> GetAllRooms();
        public RoomNameHttpDto GetRoomData(Guid roomId);
        public RoomState GetCurrentRoomState(Guid roomId);
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
            if (room == null)
            {
                throw new ResourceNotFoundException("Room with given id does not exist");
            }

            return new RoomNameHttpDto(Guid.Parse(room.Id), room.Name);
        }

        public RoomState GetCurrentRoomState(Guid roomId)
        {
            return CurrentState.GetCurrentRoomState(roomId,
                () => _roomRepository.GetRoomData(roomId) ?? throw new ResourceNotFoundException());
        }
    }
}