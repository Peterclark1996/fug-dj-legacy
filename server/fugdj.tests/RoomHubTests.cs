using System;
using System.Collections.Generic;
using System.Threading;
using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Dtos.Hub;
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

        var roomHub = BuildRoomHub(room, user, mediaChange => { mediaPlayed = mediaChange.UpNext; });

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

        var firstUsersMedia =
            new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 2), new List<int>());
        var firstUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(),
            new List<MediaWithTagsDbDto> {firstUsersMedia});

        var secondUsersMedia =
            new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 10), new List<int>());
        var secondUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(),
            new List<MediaWithTagsDbDto> {secondUsersMedia});

        MediaBeingPlayedHttpDto? mediaPlayed = null;

        var firstRoomHubConnection =
            BuildRoomHub(room, firstUser, mediaChange => { mediaPlayed = mediaChange.UpNext; });

        var secondRoomHubConnection =
            BuildRoomHub(room, secondUser, mediaChange => { mediaPlayed = mediaChange.UpNext; });

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

        var firstUsersMedia =
            new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 2), new List<int>());
        var firstUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(),
            new List<MediaWithTagsDbDto> {firstUsersMedia});

        var secondUsersMedia =
            new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 10), new List<int>());
        var secondUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(),
            new List<MediaWithTagsDbDto> {secondUsersMedia});

        MediaBeingPlayedHttpDto? mediaPlayed = null;

        var firstRoomHubConnection =
            BuildRoomHub(room, firstUser, mediaChange => { mediaPlayed = mediaChange.UpNext; });

        var secondRoomHubConnection =
            BuildRoomHub(room, secondUser, mediaChange => { mediaPlayed = mediaChange.UpNext; });

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

    [Fact]
    public void WhenMediaFinishesPlaying_NextMediaIsSentToRoomMembers()
    {
        var roomId = Guid.NewGuid();
        var room = new RoomDbDto(roomId.ToString(), Common.UniqueString());
        var hashCode = $"y{Common.UniqueString()}";
        var media = new MediaWithTagsDbDto(new MediaDbDto(hashCode, "", 1), new List<int>());
        var user = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(), new List<MediaWithTagsDbDto> {media});

        MediaBeingPlayedHttpDto? mediaFinishedBeingPlayed = null;

        var roomHub = BuildRoomHub(room, user, mediaChange =>
        {
            mediaFinishedBeingPlayed = mediaChange.JustPlayed;
        });

        roomHub.JoinRoom(roomId);
        roomHub.QueueMedia(roomId, media.Media.Player, media.Media.Code);

        mediaFinishedBeingPlayed.ShouldBeNull();

        Thread.Sleep(TimeSpan.FromSeconds(2));

        mediaFinishedBeingPlayed.ShouldNotBeNull();
        mediaFinishedBeingPlayed.Code.ShouldBe(media.Media.Code);
        mediaFinishedBeingPlayed.Player.ShouldBe(media.Media.Player);
    }
    
    [Fact]
    public void WhenMediaFinishes_AndAnotherMediaIsInTheQueue_NextMediaIsSentToRoomMembersWithJustPlayedAndUpNext()
    {
        var roomId = Guid.NewGuid();
        var room = new RoomDbDto(roomId.ToString(), Common.UniqueString());

        var firstUsersMedia =
            new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 2), new List<int>());
        var firstUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(),
            new List<MediaWithTagsDbDto> {firstUsersMedia});

        var secondUsersMedia =
            new MediaWithTagsDbDto(new MediaDbDto($"y{Common.UniqueString()}", "", 10), new List<int>());
        var secondUser = new UserDbDto(Common.UniqueString(), "", new List<TagDbDto>(),
            new List<MediaWithTagsDbDto> {secondUsersMedia});

        NextMediaHubDto? mediaPlayed = null;

        var firstRoomHubConnection =
            BuildRoomHub(room, firstUser, mediaChange => { mediaPlayed = mediaChange; });

        var secondRoomHubConnection =
            BuildRoomHub(room, secondUser, mediaChange => { mediaPlayed = mediaChange; });

        firstRoomHubConnection.JoinRoom(roomId);
        secondRoomHubConnection.JoinRoom(roomId);

        firstRoomHubConnection.QueueMedia(roomId, firstUsersMedia.Media.Player, firstUsersMedia.Media.Code);
        secondRoomHubConnection.QueueMedia(roomId, secondUsersMedia.Media.Player, secondUsersMedia.Media.Code);

        Thread.Sleep(TimeSpan.FromSeconds(3));
        
        mediaPlayed.ShouldNotBeNull();
        
        mediaPlayed.JustPlayed.ShouldNotBeNull();
        mediaPlayed.JustPlayed.Code.ShouldBe(firstUsersMedia.Media.Code);
        mediaPlayed.JustPlayed.Player.ShouldBe(firstUsersMedia.Media.Player);

        mediaPlayed.UpNext.ShouldNotBeNull();
        mediaPlayed.UpNext.Code.ShouldBe(secondUsersMedia.Media.Code);
        mediaPlayed.UpNext.Player.ShouldBe(secondUsersMedia.Media.Player);
    }

    private static RoomHub BuildRoomHub(RoomDbDto room, UserDbDto user, Action<NextMediaHubDto> onNextMedia)
    {
        var roomRepo = new Mock<IRoomRepository>();
        roomRepo.Setup(r => r.GetRoomData(Guid.Parse(room.Id))).Returns(room);
        var roomService = new RoomService(roomRepo.Object);
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(u => u.GetUser(user.Id)).Returns(user);

        var mockReceivingRoomHub = new Mock<IRoomHub>();
        mockReceivingRoomHub.Setup(r => r.NextMedia(It.IsAny<NextMediaHubDto>()))
            .Callback(onNextMedia);
        var mockClients = new Mock<IHubCallerClients<IRoomHub>>();
        mockClients.Setup(clients => clients.Group(room.Id)).Returns(mockReceivingRoomHub.Object);

        return new RoomHub(roomService, userRepo.Object)
        {
            Context = Common.HubContextWithAuthorizedUser(user.Id),
            Clients = mockClients.Object
        };
    }
}