using BookingOfflineApp.Services;
using BookingOfflineApp.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace BookingOfflineApp.Users
{
    public class AlipayFunction
    {
        private readonly IUserService _userService;
        private readonly ILogger _log;

        public AlipayFunction(IUserService userService, ILogger log)
        {
            this._userService = userService;
            this._log = log;
        }

        [FunctionName("UpdateAlipayUserInfo")]
        public async Task<IActionResult> UpdateAlipayUserInfo(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/alipay")] HttpRequest req)
        {
            string name = req.Query["name"];
            string photo = req.Query["photo"];

            var userId = req.Query["userId"];
            if (string.IsNullOrEmpty(name))
            {
                return new BadRequestResult();
            }

            if (await _userService.UpdateAlipayUserAsync(userId, name, photo))
            {
                _log.LogInformation($"alipay user {userId} updated successfully.");
                return new OkResult();
            }

            return new NotFoundResult();
        }

        [FunctionName("GetAlipayUserInfo")]
        public IActionResult GetAlipayUserInfo(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/alipay/info")] HttpRequest req)
        {
            var userId = req.Query["userId"];
            var user = _userService.GetAlipayUserInfo(userId);

            return new OkObjectResult(user);
        }

        [FunctionName("UpdateWechatUserInfo")]
        public async Task<IActionResult> UpdateWechatUserInfo(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/wechat")] HttpRequest req)
        {
            var userId = req.Query["userId"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<UserModel>(requestBody);
            if (string.IsNullOrEmpty(model?.NickName))
            {
                return new BadRequestResult();
            }

            if (await _userService.UpdateWechatUserAsync(userId, model))
            {
                _log.LogInformation($"wechat user {userId} updated successfully.");
                return new OkResult();
            }

            return new NotFoundResult();
        }

        [FunctionName("GetWechatUserInfo")]
        public IActionResult GetWechatUserInfo(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/wechat/info")] HttpRequest req)
        {
            var userId = req.Query["userId"];
            var user = _userService.GetWechatUserInfo(userId);

            return new OkObjectResult(user);
        }
    }
}
