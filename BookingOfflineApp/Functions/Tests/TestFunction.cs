using BookingOfflineApp.Common;
using BookingOfflineApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookingOfflineApp
{
    public class TestFunction
    {
        private readonly ITokenGeneratorService _tokenService;
        public TestFunction(ITokenGeneratorService tokenService)
        {
            this._tokenService = tokenService;
        }

        [FunctionName("TestRunning")]
        public IActionResult TestRunning(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test/running")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(new { State = "succeed" });
        }

        [FunctionName("TestProtected")]
        public IActionResult TestProtected(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/protected")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(new { State = "succeed" });
        }

#if DEBUG
        [FunctionName("TestGetFakeToken")]
        public IActionResult TestGetFakeToken(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/token")] HttpRequest req,
            ILogger log)
        {
            // authentication successful so generate jwt token
            var tokenStr = _tokenService.CreateJwtToken(new AlipayUser()
            {
                Id = "yufelix",
                AlibabaUserId = "1234a",
                AlipayUserId = "123456abc",
                CreatedAt = DateTime.UtcNow
            });

            return new OkObjectResult(new { token = tokenStr });
        }
#endif
    }
}
