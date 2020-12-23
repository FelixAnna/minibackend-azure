using Alipay.AopSdk.Core.Response;

namespace BookingOfflineApp.Common
{
    public interface IAlipayService
    {
        AlipaySystemOauthTokenResponse GetUserIdByCode(string authCode);
    }
}