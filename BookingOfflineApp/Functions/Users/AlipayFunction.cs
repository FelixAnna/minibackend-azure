using BookingOfflineApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingOfflineApp.Users
{
    public class AlipayFunction
    {
        private readonly IUserService _userService;
        public AlipayFunction(IUserService userService)
        {
            this._userService = userService;
        }

        [FunctionName("AlipayUpdate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/alipay")] HttpRequest req,
            ILogger log)
        {
            string name = req.Query["name"];
            string photo = req.Query["photo"];

            var userId = string.Empty;// User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(name))
            {
                return new BadRequestResult();
            }

            if (await _userService.UpdateAlipayUserAsync(userId, name, photo))
            {
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
