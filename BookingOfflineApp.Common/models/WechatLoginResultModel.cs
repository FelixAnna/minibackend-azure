using Newtonsoft.Json;

namespace BookingOfflineApp.Common.Models
{
    public class WechatLoginResultModel
    {
        //用户唯一标识
        [JsonProperty("openid")]
        public string OpenId { get; set; }

        //会话密钥
        [JsonProperty("session_key")]
        public string SessionKey { get; set; }

        //用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回，详见 UnionID 机制说明。
        [JsonProperty("unionid")]
        public string UnionId { get; set; }

        /**
         * errcode 的合法值
         * 
         * 值	说明	最低版本
         * -1	系统繁忙，此时请开发者稍候再试	
         * 0	请求成功	
         * 40029	code 无效	
         * 45011	频率限制，每个用户每分钟100次
        */
        //错误码
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }

        //错误信息
        [JsonProperty("errmsg")]
        public string ErrorMsg { get; set; }

        public bool IsError
        {
            get
            {
                return ErrorCode != 0;
            }
        }
    }
}
