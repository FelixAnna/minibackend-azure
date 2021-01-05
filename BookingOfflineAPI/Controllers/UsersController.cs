using BookingOfflineApp.Services;
using BookingOfflineApp.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingOfflineApp.Web.Controllers
{
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _log;

        public UsersController(IUserService userService, ILogger<UsersController> log)
        {
            this._userService = userService;
            this._log = log;
        }

        [Authorize]
        [HttpPost("alipay")]
        public async Task<IActionResult> UpdateAlipayUserAsync([FromQuery] string name, [FromQuery] string photo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            if (await _userService.UpdateAlipayUserAsync(userId, name, photo))
            {
                _log.LogInformation($"alipay user {userId} updated successfully.");
                return Ok();
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("alipay/info")]
        public IActionResult GetAlipayUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetAlipayUserInfo(userId);

            return Ok(user);
        }

        [Authorize]
        [HttpPost("wechat")]
        public async Task<IActionResult> UpdateWechatUserAsync([FromBody] UserModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(model?.NickName))
            {
                return BadRequest();
            }

            if (await _userService.UpdateWechatUserAsync(userId, model))
            {
                _log.LogInformation($"wechat user {userId} updated successfully.");
                return Ok();
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("wechat/info")]
        public IActionResult GetWechatUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetWechatUserInfo(userId);

            return Ok(user);
        }
    }
}
