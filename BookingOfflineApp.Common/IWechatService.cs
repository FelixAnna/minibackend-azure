using BookingOfflineApp.Common.Models;
using System.Threading.Tasks;

namespace BookingOfflineApp.Common
{
    public interface IWechatService
    {
        Task<WechatLoginResultModel> GetUserIdByCode(string authCode);
    }
}
