using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using BookingOfflineApp.Services.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BookingOfflineApp.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository<AlipayUser> _userRepo;
        private readonly IUserRepository<WechatUser> _wechatUserRepo;

        public UserService(IUserRepository<AlipayUser> userRepo,
            IUserRepository<WechatUser> wechatUserRepo,
            ILogger<UserService> logger)
        {
            _logger = logger;
            _userRepo = userRepo;
            _wechatUserRepo = wechatUserRepo;
        }

        public UserResultModel GetAlipayUserInfo(string userId)
        {
            var user = _userRepo.FindById(userId);
            return new AlipayUserModel()
            {
                Id=user.Id,
                NickName=user.AlipayName,
                AvatarUrl = user.AlipayPhoto,
                AlibabaUserId =user.AlibabaUserId,
                AlipayUserId=user.AlipayUserId,
                CreatedAt=user.CreatedAt
            };
        }

        public async Task<bool> UpdateAlipayUserAsync(string userId, string nickName, string photo)
        {
            var user = _userRepo.FindById(userId);
            if(user == null)
            {
                _logger.LogError($"Alipay User {userId} not exists.");
                return false;
            }

            user.AlipayName = nickName;
            user.AlipayPhoto = photo;
            await _userRepo.UpdateAsync(user);
            return true;
        }

        public UserResultModel GetWechatUserInfo(string userId)
        {
            var user = _wechatUserRepo.FindById(userId);
            return new WechatUserModel()
            {
                Id = user.Id,
                NickName = user.NickName,
                AvatarUrl = user.AvatarUrl,
                OpenId = user.OpenId,
                UnionId = user.UnionId,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> UpdateWechatUserAsync(string userId, UserModel model)
        {
            var user = _wechatUserRepo.FindById(userId);
            if (user == null)
            {
                _logger.LogError($"Wechat User {userId} not exists.");
                return false;
            }

            user.NickName = model.NickName;
            user.AvatarUrl = model.AvatarUrl;
            user.Country = model.Country;
            user.Province = model.Province;
            user.City = model.City;
            user.Gender = model.Gender;
            user.Language = model.Language;
            await _wechatUserRepo.UpdateAsync(user);
            return true;
        }
    }
}
