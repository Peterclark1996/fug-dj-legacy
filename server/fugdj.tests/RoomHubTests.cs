using System;
using System.Collections.Generic;
using System.Threading;
using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Hubs;
using fugdj.Repositories;
using fugdj.Services;
using fugdj.tests.Helpers;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Shouldly;
using Xunit;

namespace fugdj.tests;

public class RoomHubTests
{
    [Fact]
    public void WhenMediaIsQueued_AndTheQueueIsEmpty_MediaIsPlayedToRoomMembers()
    {
        var roomId = Guid.NewGuid();
        var room = new RoomDbDto(roomId.ToString(), Common.UniqueString());
        var hashCode = $"y{Common.UniqueString()}";
        var media = new MediaWithTagsDbDto(new MediaDbDto(hashCode, "", 10), new List<int>());
        var user = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(), new List<MediaWithTagsDbDto> {media});

        MediaBeingPlayedHttpDto? mediaPlayed = null;
        
        var roomHub = BuildRoomHub(room, user, mediaToPlay =>
        {
            mediaPlayed = mediaToPlay;
        });
        
        roomHub.JoinRoom(roomId);
        roomHub.QueueMedia(roomId, media.Media.Player, media.Media.Code);

        mediaPlayed.ShouldNotBeNull();
        mediaPlayed.Code.ShouldBe(media.Media.Code);
        mediaPlayed.Player.ShouldBe(media.Media.Player);
    }

    [Fact]
    public void WhenMediaFinishes_NextMediaInQueueIsPlayedToRoomMembers()
    {
        var roomId = Guid.NewGuid();
        var room = new RoomDbDto(roomId.ToString(), Common.UniqueString());

        var firstUsersMedia = new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 2), new List<int>());
        var firstUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(), new List<MediaWithTagsDbDto> {firstUsersMedia});
        
        var secondUsersMedia = new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 10), new List<int>());
        var secondUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(), new List<MediaWithTagsDbDto> {secondUsersMedia});

        MediaBeingPlayedHttpDto? mediaPlayed = null;
        
        var firstRoomHubConnection = BuildRoomHub(room, firstUser, mediaToPlay =>
        {
            mediaPlayed = mediaToPlay;
        });
        
        var secondRoomHubConnection = BuildRoomHub(room, secondUser, mediaToPlay =>
        {
            mediaPlayed = mediaToPlay;
        });
        
        firstRoomHubConnection.JoinRoom(roomId);
        secondRoomHubConnection.JoinRoom(roomId);
        
        firstRoomHubConnection.QueueMedia(roomId, firstUsersMedia.Media.Player, firstUsersMedia.Media.Code);
        secondRoomHubConnection.QueueMedia(roomId, secondUsersMedia.Media.Player, secondUsersMedia.Media.Code);

        mediaPlayed.ShouldNotBeNull();
        mediaPlayed.Code.ShouldBe(firstUsersMedia.Media.Code);
        mediaPlayed.Player.ShouldBe(firstUsersMedia.Media.Player);
        
        Thread.Sleep(TimeSpan.FromSeconds(3));
        
        mediaPlayed.ShouldNotBeNull();
        mediaPlayed.Code.ShouldBe(secondUsersMedia.Media.Code);
        mediaPlayed.Player.ShouldBe(secondUsersMedia.Media.Player);
    }
    
    [Fact]
    public void WhenMediaIsQueued_AndLastMediaPlayedHasFinished_MediaIsPlayedToRoomMembers()
    {
        var roomId = Guid.NewGuid();
        var room = new RoomDbDto(roomId.ToString(), Common.UniqueString());

        var firstUsersMedia = new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 2), new List<int>());
        var firstUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(), new List<MediaWithTagsDbDto> {firstUsersMedia});
        
        var secondUsersMedia = new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 10), new List<int>());
        var secondUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(), new List<MediaWithTagsDbDto> {secondUsersMedia});

        MediaBeingPlayedHttpDto? mediaPlayed = null;
        
        var firstRoomHubConnection = BuildRoomHub(room, firstUser, mediaToPlay =>
        {
            mediaPlayed = mediaToPlay;
        });
        
        var secondRoomHubConnection = BuildRoomHub(room, secondUser, mediaToPlay =>
        {
            mediaPlayed = mediaToPlay;
        });
        
        firstRoomHubConnection.JoinRoom(roomId);
        secondRoomHubConnection.JoinRoom(roomId);
        
        firstRoomHubConnection.QueueMedia(roomId, firstUsersMedia.Media.Player, firstUsersMedia.Media.Code);

        mediaPlayed.ShouldNotBeNull();
        mediaPlayed.Code.ShouldBe(firstUsersMedia.Media.Code);
        mediaPlayed.Player.ShouldBe(firstUsersMedia.Media.Player);
        
        Thread.Sleep(TimeSpan.FromSeconds(3));
        
        secondRoomHubConnection.QueueMedia(roomId, secondUsersMedia.Media.Player, secondUsersMedia.Media.Code);
        
        mediaPlayed.ShouldNotBeNull();
        mediaPlayed.Code.ShouldBe(secondUsersMedia.Media.Code);
        mediaPlayed.Player.ShouldBe(secondUsersMedia.Media.Player);
    }

    private static RoomHub BuildRoomHub(RoomDbDto room, UserDbDto user, Action<MediaBeingPlayedHttpDto> onPlayMedia)
    {
        var roomRepo = new Mock<IRoomRepository>();
        roomRepo.Setup(r => r.GetRoomData(Guid.Parse(room.Id))).Returns(room);
        var roomService = new RoomService(roomRepo.Object);
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(u => u.GetUser(user.Id)).Returns(user);

        var mockReceivingRoomHub = new Mock<IRoomHub>();
        mockReceivingRoomHub.Setup(r => r.PlayMedia(It.IsAny<MediaBeingPlayedHttpDto>()))
            .Callback(onPlayMedia);
        var mockClients = new Mock<IHubCallerClients<IRoomHub>>();
        mockClients.Setup(clients => clients.Group(room.Id)).Returns(mockReceivingRoomHub.Object);
        
        return new RoomHub(roomService, userRepo.Object)
        {
            Context = Common.HubContextWithAuthorizedUser(user.Id),
            Clients = mockClients.Object
        };
    }
}