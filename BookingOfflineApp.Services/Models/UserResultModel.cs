using System;
using System.Collections.Generic;
using System.Text;

namespace BookingOfflineApp.Services.Models
{
    public class AlipayUserModel : UserResultModel
    {
        public string AlibabaUserId { get; set; }

        public string AlipayUserId { get; set; }
    }

    public class UserResultModel
    {
        public string Id { get; set; }

        public string NickName { get; set; }
        public string AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class WechatUserModel : UserResultModel
    {
        public string OpenId { get; set; }

        public string UnionId { get; set; }
    }
}
