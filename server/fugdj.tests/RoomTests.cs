using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Controllers;
using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Repositories;
using fugdj.Services;
using fugdj.tests.Helpers;
using fugdj.tests.Mocks;
using Moq;
using Shouldly;
using Xunit;

namespace fugdj.tests;

public class RoomTests
{
    [Fact]
    public void WhenGettingAllRooms_RoomsAreReturned()
    {
        var firstExpectedRoom = new RoomNameHttpDto(Guid.NewGuid(), Common.UniqueString());
        var secondExpectedRoom = new RoomNameHttpDto(Guid.NewGuid(), Common.UniqueString());
        
        var roomService = new RoomService(new RoomRepositoryMock(new List<RoomDbDto>
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
        var roomService = new RoomService(new RoomRepositoryMock(new List<RoomDbDto>()));
        var roomController = new RoomController(roomService);

        var result = roomController.GetAll().GetResponseObject<IEnumerable<RoomNameHttpDto>>();
        result.ShouldBeEmpty();
    }

    [Fact]
    public void WhenGettingExistingRoom_RoomIsReturned()
    {
        var expectedRoom = new RoomNameHttpDto(Guid.NewGuid(), Common.UniqueString());

        var roomService = new RoomService(new RoomRepositoryMock(new List<RoomDbDto>
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

        var roomService = new RoomService(new RoomRepositoryMock(new List<RoomDbDto>()));
        var roomController = new RoomController(roomService);

        Should.Throw<ResourceNotFoundException>(
            () => roomController.Get(nonExistingRoom.Id.ToString())
        );
    }
}