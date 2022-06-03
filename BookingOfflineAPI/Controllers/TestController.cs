using BookingOfflineApp.Common;
using BookingOfflineApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookingOfflineApp.Web.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly ITokenGeneratorService _tokenService;
        public TestController(ITokenGeneratorService tokenService)
        {
            this._tokenService = tokenService;
        }
#if DEBUG
        [AllowAnonymous]
        [HttpGet("token")]
        public IActionResult Index()
        {
            // authentication successful so generate jwt token
            var tokenStr = _tokenService.CreateJwtToken(new AlipayUser()
            {
                Id = "yufelix",
                AlibabaUserId = "1234a",
                AlipayUserId = "123456abc",
                CreatedAt = DateTime.UtcNow
            });
            return Ok(new { token = tokenStr });
        }
#endif

        [HttpGet("running")]
        [AllowAnonymous]
        public IActionResult Running()
        {
            return Ok(new { State = "succeed" });
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            return Ok(new { State = "succeed" });
        }

        [HttpGet("unprotected")]
        [AllowAnonymous]
        public IActionResult UnProtected()
        {
            return Ok(new { State = "succeed" });
        }

        [HttpGet("version")]
        [AllowAnonymous]
        public IActionResult Version()
        {
            return Ok(new { State = "220603" });
        }
    }
}