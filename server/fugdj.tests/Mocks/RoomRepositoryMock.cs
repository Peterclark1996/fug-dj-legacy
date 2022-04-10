using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Dtos.Db;
using fugdj.Repositories;

namespace fugdj.tests.Mocks;

public class RoomRepositoryMock : IRoomRepository
{
    private readonly List<RoomDbDto> _rooms;

    public RoomRepositoryMock(List<RoomDbDto> rooms)
    {
        _rooms = rooms;
    }

    public IEnumerable<RoomDbDto> GetAllRooms() => _rooms;

    public RoomDbDto? GetRoomData(Guid roomId) => _rooms.SingleOrDefault(r => Guid.Parse(r.Id) == roomId);
}