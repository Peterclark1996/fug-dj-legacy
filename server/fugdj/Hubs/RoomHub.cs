using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fugdj.Dtos.Http;
using fugdj.Dtos.Hub;
using fugdj.Extensions;
using fugdj.Repositories;
using fugdj.Services;
using fugdj.State;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace fugdj.Hubs
{
    public interface IRoomHub
    {
        Task NextMedia(NextMediaHubDto mediaToPlay);
        Task UpdateRoomUsers(List<ConnectedUserHubDto> roomUsers);
    }

    public class RoomHub : Hub<IRoomHub>
    {
        private readonly IRoomService _roomService;
        private readonly IUserRepository _userRepository;

        public RoomHub(IRoomService roomService, IUserRepository userRepository)
        {
            _roomService = roomService;
            _userRepository = userRepository;
        }

        [Authorize]
        public Task QueueMedia(Guid roomId, Player player, string code)
        {
            var userId = Context.GetAuthorizedUserId();
            var user = _userRepository.GetUser(userId);

            var mediaWithTags = user?.MediaList
                .SingleOrDefault(m => m.Media.HashCode.GetPlayer() == player && m.Media.HashCode.GetCode() == code);
            if (mediaWithTags == null) return Task.CompletedTask;

            var room = _roomService.GetCurrentRoomState(roomId);
            room.QueueMedia(
                mediaWithTags.Media,
                userId,
                media => Clients.Group(roomId.ToString()).NextMedia(media)
            );

            return Task.CompletedTask;
        }

        [Authorize]
        public async Task JoinRoom(Guid roomId)
        {
            var userId = Context.GetAuthorizedUserId();
            CurrentState.GetAllActiveRoomStates().ForEach(r => r.RemoveUser(userId));

            var user = _userRepository.GetUser(userId);
            if (user == null) throw new ResourceNotFoundException();

            var currentRoom = _roomService.GetCurrentRoomState(roomId);
            currentRoom.AddOrUpdateUser(
                new ConnectedUserHubDto(
                    userId,
                    user.Name,
                    Utility.RandomNumberBetween(10, 90),
                    Utility.RandomNumberBetween(10, 90)
                )
            );

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            await BroadcastUpdatedRoomUsers(roomId);
        }

        public async Task BroadcastUpdatedRoomUsers(Guid roomId)
        {
            var currentRoom = _roomService.GetCurrentRoomState(roomId);
            await Clients.Group(roomId.ToString()).UpdateRoomUsers(currentRoom.GetUsers().ToList());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.GetAuthorizedUserId();
            CurrentState.GetAllActiveRoomStates().ForEach(r => r.RemoveUser(userId));
            await base.OnDisconnectedAsync(exception);
        }
    }
}