using fugdj.Dtos.Http;
using fugdj.Services;
using Microsoft.AspNetCore.Mvc;

namespace fugdj.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }
        
    [HttpGet]
    public IEnumerable<RoomNameHttpDto> GetAll()
    {
        return _roomService.GetAllRooms();
    }
        
    [HttpGet]
    public RoomNameHttpDto Get([FromQuery] string roomId)
    {
        return _roomService.GetRoomData(Guid.Parse(roomId));
    }
}