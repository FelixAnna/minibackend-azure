using BookingOfflineApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookingOfflineApp.Common
{
    public interface IWechatService
    {
        Task<WechatLoginResultModel> GetUserIdByCode(string authCode);
    }
}
