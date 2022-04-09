using fugdj.Dtos.Http;
using fugdj.Extensions;
using fugdj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fugdj.Controllers;

[ApiController]
[Route("api/user/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        var userId = Request.GetAuthorizedUserId();
        return Ok(_userService.GetUser(userId));
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddMedia([FromBody] MediaHashCodeHttpDto mediaToAdd)
    {
        var userId = Request.GetAuthorizedUserId();
        _userService.AddMediaForUser(userId, mediaToAdd);
        return Ok();
    }
    
    [HttpDelete]
    [Authorize]
    public IActionResult DeleteMedia([FromQuery] string media)
    {
        var userId = Request.GetAuthorizedUserId();
        _userService.DeleteMediaForUser(userId, media);
        return Ok();
    }
}