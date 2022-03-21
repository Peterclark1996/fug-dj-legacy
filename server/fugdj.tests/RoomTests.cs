using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Controllers;
using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Repositories;
using fugdj.Services;
using Shouldly;
using Xunit;

namespace fugdj.tests;

public class RoomRepositoryFake : IRoomRepository
{
    private readonly List<RoomDbDto> _rooms;

    public RoomRepositoryFake(List<RoomDbDto> rooms)
    {
        _rooms = rooms;
    }

    public IEnumerable<RoomDbDto> GetAllRooms() => _rooms;

    public RoomDbDto? GetRoomData(Guid roomId) => _rooms.SingleOrDefault(r => Guid.Parse(r.Id) == roomId);
}

public class RoomTests
{
    [Fact]
    public void WhenGettingAllRooms_RoomsAreReturned()
    {
        var firstExpectedRoom = new RoomNameHttpDto(Guid.NewGuid(), Common.UniqueString());
        var secondExpectedRoom = new RoomNameHttpDto(Guid.NewGuid(), Common.UniqueString());

        var roomService = new RoomService(new RoomRepositoryFake(new List<RoomDbDto>
        {
            new(firstExpectedRoom.Id.ToString(), firstExpectedRoom.Name),
            new(secondExpectedRoom.Id.ToString(), secondExpectedRoom.Name)
        }));
        var roomController = new RoomController(roomService);

        var result = roomController.GetAll().GetResponseObject<IEnumerable<RoomNameHttpDto>>().ToList();
        result.ShouldHaveCount(2);
        result.ShouldContainEquivalent(firstExpectedRoom);
        result.ShouldContainEquivalent(secondExpectedRoom);
    }
    
    [Fact]
    public void WhenGettingAllRooms_WhenNoneExist_EmptyListIsReturned()
    {
        var roomService = new RoomService(new RoomRepositoryFake(new List<RoomDbDto>()));
        var roomController = new RoomController(roomService);

        var result = roomController.GetAll().GetResponseObject<IEnumerable<RoomNameHttpDto>>();
        result.ShouldBeEmpty();
    }

    [Fact]
    public void WhenGettingExistingRoom_RoomIsReturned()
    {
        var expectedRoom = new RoomNameHttpDto(Guid.NewGuid(), Common.UniqueString());

        var roomService = new RoomService(new RoomRepositoryFake(new List<RoomDbDto>
            {new(expectedRoom.Id.ToString(), expectedRoom.Name)}));
        var roomController = new RoomController(roomService);

        var result = roomController.Get(expectedRoom.Id.ToString()).GetResponseObject<RoomNameHttpDto>();
        result.Id.ShouldBe(expectedRoom.Id);
        result.Name.ShouldBe(expectedRoom.Name);
    }

    [Fact]
    public void WhenGettingNonExistingRoom_404IsReturned()
    {
        var nonExistingRoom = new RoomNameHttpDto(Guid.NewGuid(), Common.UniqueString());

        var roomService = new RoomService(new RoomRepositoryFake(new List<RoomDbDto>()));
        var roomController = new RoomController(roomService);

        try
        {
            roomController.Get(nonExistingRoom.Id.ToString());
            Common.FailTest();
        }
        catch (Exception e)
        {
            e.GetType().ShouldBe(new ResourceNotFoundException().GetType());
        }
    }
}