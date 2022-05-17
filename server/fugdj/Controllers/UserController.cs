using fugdj.Dtos.Http;
using fugdj.Extensions;
using fugdj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fugdj.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
    
        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public IActionResult Get()
        {
            var userId = Request.GetAuthorizedUserId();
            return Ok(_userService.GetUser(userId));
        }

        [HttpPatch]
        [Authorize]
        public IActionResult UpdateUser(UserUpdateHttpDto userUpdate)
        {
            var userId = Request.GetAuthorizedUserId();
            _userService.UpdateUser(userId, userUpdate);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public IActionResult CreateMediaTag([FromBody] CreateMediaTagHttpDto tagToCreate)
        {
            var userId = Request.GetAuthorizedUserId();
            _userService.CreateTagForMedia(userId, tagToCreate.MediaToAddTagTo, tagToCreate.TagName);
            return Ok();
        }
    }
}