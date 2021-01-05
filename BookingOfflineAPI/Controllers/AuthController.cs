using BookingOfflineApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BookingOfflineApp.Web.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ILogger<AuthController> _log;

        public AuthController(ILoginService loginService, ILogger<AuthController> log)
        {
            this._loginService = loginService;
            this._log = log;
        }

        [HttpPost("alipay/login")]
        [AllowAnonymous]
        public IActionResult LoginAlipay([FromQuery] string code)
        {
            var response = _loginService.LoginMiniAlipay(code);
            if (response == null)
            {
                _log.LogWarning($"Failed to login by alipay code: {code}.");
                return Unauthorized();
            }

            _log.LogInformation($"Logged on by alipay code: {code}.");

            return Ok(response);
        }

        [HttpPost("wechat/login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWechat([FromQuery] string code)
        {
            var response = await _loginService.LoginMiniWechatAsync(code);
            if (response == null)
            {
                _log.LogWarning($"Failed to login by wechat code: {code}.");
                return Unauthorized();
            }

            _log.LogInformation($"Logged on by wechat code: {code}.");
            return Ok(response);
        }
    }
}
