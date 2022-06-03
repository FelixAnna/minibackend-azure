using BookingOfflineApp.Common;
using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using BookingOfflineApp.Services.Interfaces;
using BookingOfflineApp.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookingOfflineApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly ITokenGeneratorService _tokenService;

        private readonly IAlipayService _alipayService;
        private readonly IUserRepository<AlipayUser> _userRepo;

        private readonly IWechatService _wechatService;
        private readonly IUserRepository<WechatUser> _wechatUserRepo;

        public LoginService(ITokenGeneratorService tokenService,
            IAlipayService alipayService,
            IUserRepository<AlipayUser> userRepo,
            IWechatService wechatService,
            IUserRepository<WechatUser> wechatUserRepo,
            ILogger<LoginService> logger)
        {
            _logger = logger;
            this._tokenService = tokenService;
            this._alipayService = alipayService;
            this._wechatService = wechatService; ;
            this._wechatUserRepo = wechatUserRepo;
            _userRepo = userRepo;
        }

        public LoginResultModel LoginMiniAlipay(string code)
        {
            var response = _alipayService.GetUserIdByCode(code);
            if (response.IsError)
            {
                _logger.LogError(response.Body);
                return null;
            }

            var alipayUser = _userRepo.FindByOpenId(response.AlipayUserId);
            if (alipayUser == null)
            {
                alipayUser = _userRepo.Create(new AlipayUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    AlibabaUserId = response.UserId,
                    AlipayUserId = response.AlipayUserId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            var tokenStr = _tokenService.CreateJwtToken(alipayUser);
            var result = new LoginResultModel()
            {
                BOToken = tokenStr,
                UserId = alipayUser.Id,
                NickName = alipayUser.AlipayName,
                AvatarUrl = alipayUser.AlipayPhoto
            };

            return result;
        }

        public async Task<LoginResultModel> LoginMiniWechatAsync(string code)
        {
            var response = await _wechatService.GetUserIdByCode(code);
            //retry
            int retryCount = 0;
            while (response.ErrorCode < 0 && retryCount++ < 3)
            {
                response = await _wechatService.GetUserIdByCode(code);
            }

            if (response.IsError)
            {
                _logger.LogError(response.ErrorMsg);
                return null;
            }

            var wechatUser = _wechatUserRepo.FindByOpenId(response.OpenId);
            if (wechatUser == null)
            {
                wechatUser = _wechatUserRepo.Create(new WechatUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    OpenId = response.OpenId,
                    UnionId = response.UnionId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            var tokenStr = _tokenService.CreateJwtToken(wechatUser);
            var result = new LoginResultModel()
            {
                BOToken = tokenStr,
                //AccessToken = response.SessionKey,
                UserId = wechatUser.Id,
                NickName = wechatUser.NickName,
                AvatarUrl = wechatUser.AvatarUrl
            };

            return result;
        }
    }
}
