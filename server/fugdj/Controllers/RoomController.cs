using fugdj.Services;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public IActionResult GetAll()
    {
        return Ok(_roomService.GetAllRooms());
    }
        
    [HttpGet]
    [Authorize]
    public IActionResult Get([FromQuery] string roomId)
    {
        return Ok(_roomService.GetRoomData(Guid.Parse(roomId)));
    }
}