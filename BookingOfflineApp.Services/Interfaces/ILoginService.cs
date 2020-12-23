using BookingOfflineApp.Services.Models;
using System.Threading.Tasks;

namespace BookingOfflineApp.Services.Interfaces
{
    public interface ILoginService
    {
        LoginResultModel LoginMiniAlipay(string code);
        Task<LoginResultModel> LoginMiniWechatAsync(string code);
    }
}