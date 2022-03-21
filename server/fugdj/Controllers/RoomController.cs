using fugdj.Services;
using Microsoft.AspNetCore.Mvc;

namespace fugdj.Controllers;

[ApiController]
[Route("api/rooms/[action]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }
        
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_roomService.GetAllRooms());
    }
        
    [HttpGet]
    public IActionResult Get([FromQuery] string roomId)
    {
        return Ok(_roomService.GetRoomData(Guid.Parse(roomId)));
    }
}