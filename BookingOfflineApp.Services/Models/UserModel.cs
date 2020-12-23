using System;
using System.Collections.Generic;
using System.Text;

namespace BookingOfflineApp.Services.Models
{
    public class UserModel
    {
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 0: 位置, 1: 男, 2: 女
        /// </summary>
        public int Gender { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        /// <summary>
        /// en/zh_CN/zh_TW
        /// </summary>
        public string Language { get; set; }
    }
}
