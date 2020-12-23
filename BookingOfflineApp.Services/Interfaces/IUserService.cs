using BookingOfflineApp.Services.Models;
using System.Threading.Tasks;

namespace BookingOfflineApp.Services
{
    public interface IUserService
    {
        Task<bool> UpdateAlipayUserAsync(string userId, string nickName, string photo);
        UserResultModel GetAlipayUserInfo(string userId);

        Task<bool> UpdateWechatUserAsync(string userId, UserModel model);
        UserResultModel GetWechatUserInfo(string userId);
    }
}