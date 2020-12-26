using BookingOfflineApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BookingOfflineApp.Functions.Auth
{
    public class AuthFunction
    {
        private readonly ILoginService _loginService;
        private readonly ILogger<AuthFunction> _log;
        public AuthFunction(ILoginService loginService, ILogger<AuthFunction> log)
        {
            this._loginService = loginService;
            this._log = log;
        }

        //TO DO: changed from get to POST in front-end
        [FunctionName("LoginAliPay")]
        public async Task<IActionResult> LoginAliPay(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/alipay/login")] HttpRequest req)
        {
            string code = req.Query["code"];

            var response = _loginService.LoginMiniAlipay(code);
            if (response == null)
            {
                _log.LogWarning($"Failed to login by alipay code: {code}.");
                return new UnauthorizedResult();
            }

            _log.LogInformation($"Logged on by alipay code: {code}.");
            return new OkObjectResult(response);
        }

        //TO DO: changed from get to POST in front-end
        [FunctionName("LoginWechat")]
        public async Task<IActionResult> LoginWechat(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/wechat/login")] HttpRequest req)
        {
            string code = req.Query["code"];

            var response = await _loginService.LoginMiniWechatAsync(code);
            if (response == null)
            {
                _log.LogWarning($"Failed to login by wechat code: {code}.");
                return new UnauthorizedResult();
            }

            _log.LogInformation($"Logged on by wechat code: {code}.");
            return new OkObjectResult(response);
        }
    }
}
