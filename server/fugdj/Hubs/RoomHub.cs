using fugdj.Dtos;
using fugdj.Dtos.Http;
using fugdj.Extensions;
using fugdj.Services;
using fugdj.State;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace fugdj.Hubs;

public interface IRoomHub
{
    Task PlayMedia(QueuedMedia mediaToPlay);
}

public class RoomHub : Hub<IRoomHub>
{
    private readonly IRoomService _roomService;
    private readonly IUserService _userService;

    public RoomHub(IRoomService roomService, IUserService userService)
    {
        _roomService = roomService;
        _userService = userService;
    }
    
    [Authorize]
    public Task QueueMedia(Guid roomId, Player player, string code)
    {
        var userId = Context.GetAuthorizedUserId();

        var media = _userService.GetUser(userId).Media
            .SingleOrDefault(m => m.Player == player && m.Code == code);
        if (media == null) return Task.CompletedTask;

        var room = _roomService.GetCurrentRoomState(roomId);
        room.QueueMedia(media, userId);

        return Task.CompletedTask;
    }

    public async Task JoinRoom(Guid roomId, string authToken)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
}