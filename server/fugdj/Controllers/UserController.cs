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
    public IActionResult CreateMedia([FromBody] MediaHashCodeHttpDto mediaToAdd)
    {
        var userId = Request.GetAuthorizedUserId();
        _userService.AddMediaForUser(userId, mediaToAdd);
        return Ok();
    }
    
    [HttpPost]
    [Authorize]
    public IActionResult CreateMediaTag([FromBody] CreateMediaTagHttpDto tagToCreate)
    {
        var userId = Request.GetAuthorizedUserId();
        _userService.CreateTagForMedia(userId, tagToCreate.MediaToAddTagTo, tagToCreate.TagName);
        return Ok();
    }
    
    [HttpPatch]
    [Authorize]
    public IActionResult UpdateMedia([FromBody] MediaHttpDto mediaToUpdate)
    {
        var userId = Request.GetAuthorizedUserId();
        _userService.UpdateMediaForUser(userId, mediaToUpdate);
        return Ok();
    }
    
    [HttpPost]
    [Authorize]
    public IActionResult DeleteMedia([FromBody] MediaHashCodeHttpDto media)
    {
        var userId = Request.GetAuthorizedUserId();
        _userService.DeleteMediaForUser(userId, media);
        return Ok();
    }
}