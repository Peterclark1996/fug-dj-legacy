using fugdj.Dtos.Http;
using fugdj.Extensions;
using fugdj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fugdj.Controllers
{
    [ApiController]
    [Route("api/media/{id}")]
    public class MediaController : ControllerBase
    {
        private readonly IUserService _userService;

        public MediaController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateMedia(string id)
        {
            var userId = Request.GetAuthorizedUserId();
            var mediaId = id.GetMediaHashCodeAsObject();
            _userService.AddMediaForUser(userId, mediaId);
            return Ok();
        }

        [HttpPatch]
        [Authorize]
        public IActionResult UpdateMedia([FromBody] MediaUpdateHttpDto mediaToUpdate, string id)
        {
            var userId = Request.GetAuthorizedUserId();
            var mediaId = id.GetMediaHashCodeAsObject();
            _userService.UpdateMediaForUser(userId, mediaToUpdate, mediaId);
            return Ok();
        }
    
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteMedia(string id)
        {
            var userId = Request.GetAuthorizedUserId();
            var mediaId = id.GetMediaHashCodeAsObject();
            _userService.DeleteMediaForUser(userId, mediaId);
            return Ok();
        }
    }
}