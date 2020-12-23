using BookingOfflineApp.Entities;

namespace BookingOfflineApp.Common
{
    public interface ITokenGeneratorService
    {
        string CreateJwtToken(AlipayUser alipayUser);
        string CreateJwtToken(WechatUser alipayUser);
    }
}