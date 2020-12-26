using BookingOfflineApp.Common;
using BookingOfflineApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BookingOfflineApp
{
    public class TestFunction
    {
        private readonly ITokenGeneratorService _tokenService;
        private readonly ILogger<TestFunction> _log;
        public TestFunction(ITokenGeneratorService tokenService, ILogger<TestFunction> log)
        {
            this._tokenService = tokenService;
            this._log = log;
        }

        [FunctionName("TestRunning")]
        public IActionResult TestRunning(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test/running")] HttpRequest req)
        {
            return new OkObjectResult(new { State = "succeed" });
        }

        [FunctionName("TestProtected")]
        public IActionResult TestProtected(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/protected")] HttpRequest req)
        {
            return new OkObjectResult(new { State = "succeed" });
        }

        [FunctionName("TestGetFakeToken")]
        public IActionResult TestGetFakeToken(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/token")] HttpRequest req)
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

        [FunctionName("TestParseToken")]
        public IActionResult TestParseToken(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/token/parse")] HttpRequest req)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("header parameters:");
            foreach (var key in req.Headers.Keys)
            {
                result.AppendLine($"{key}:{req.Headers[key]}");
            }

            result.AppendLine("query parameters:");
            foreach (var key in req.Query.Keys)
            {
                result.AppendLine($"{key}:{req.Query[key]}");
            }
            
            return new OkObjectResult(new { Header = result.ToString() });
        }
    }
}
