using fugdj.Dtos.Http;
using fugdj.Extensions;
using fugdj.Repositories;
using fugdj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace fugdj.Hubs;

public interface IRoomHub
{
    void PlayMedia(MediaBeingPlayedHttpDto mediaToPlay);
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
        room.QueueMedia(mediaWithTags.Media, userId, () =>
        {
            Clients.Group(roomId.ToString()).PlayMedia(new MediaBeingPlayedHttpDto(mediaWithTags.Media.Name,
                mediaWithTags.Media.Player, mediaWithTags.Media.Code, userId));
        });

        return Task.CompletedTask;
    }

    [Authorize]
    public async Task JoinRoom(Guid roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }
}