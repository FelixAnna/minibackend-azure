using Alipay.AopSdk.Core.Response;
using BookingOfflineApp.Common.Models;
using BookingOfflineApp.Entities;
using System.Collections.Generic;

namespace BookingOfflineApp.Services.Tests
{
    public static class FakeDataHelper
    {
        public static AlipaySystemOauthTokenResponse GetFakeAlipayResponse(bool success = false)
        {
            if (!success)
            {
                return new Alipay.AopSdk.Core.Response.AlipaySystemOauthTokenResponse()
                {
                    Code = "20000",
                    SubCode = "isp.unknow-error",
                    Msg = "Service Currently Unavailable",
                    SubMsg = "系统繁忙"
                };
            }

            return new AlipaySystemOauthTokenResponse()
            {
                UserId = "anyId",
                AlipayUserId = "anyalipayid",
                AccessToken = "anytoken",
                ExpiresIn = "anytime",
                ReExpiresIn = "anytime",
                RefreshToken = "anyretoken",
            };
        }

        public static AlipayUser GetFakeAlipayUserById(bool success = false)
        {
            if (!success)
            {
                return null;
            }

            return new AlipayUser();
        }

        public static WechatLoginResultModel GetFakeWechatLoginResultModel(bool success = false)
        {
            if (!success)
            {
                return new WechatLoginResultModel()
                {
                    ErrorCode = 40029,
                    ErrorMsg = "code 无效"
                };
            }

            return new WechatLoginResultModel()
            {
                OpenId = "anyId",
                SessionKey = "anysessionid",
                UnionId = "anytoken",
                ErrorCode = 0,
                ErrorMsg = null
            };
        }

        public static WechatUser GetFakeWechatUserById(bool success = false)
        {
            if (!success)
            {
                return null;
            }

            return new WechatUser();
        }

        public static Order GetFakeOrder(bool success = true)
        {
            if (success)
            {
                return new Order() { State = 1, Options = @"[{id: 1, name: '加饭', type:'bool', default: false, order: 1},
      {id: 2, name: '加辣', type:'bool', default: false, order: 2}]", OrderItems = new List<OrderItem>() };
            }

            return null;
        }

        public static OrderItem GetFakeOrderItem(bool success = true)
        {
            if (success)
            {
                return new OrderItem()
                {
                    OrderItemOptions = new List<OrderItemOption>()
                };
            }

            return null;
        }
    }
}
