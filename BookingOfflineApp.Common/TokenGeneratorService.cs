using BookingOfflineApp.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingOfflineApp.Common
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly int _expiresIn = 72;
        private readonly IConfiguration _configuration;
        public TokenGeneratorService(IConfiguration configuration)
        {
            this._configuration = configuration;
            if (int.TryParse("ExpiresIn", out int result))
            {
                _expiresIn = result;
            }
        }

        public string CreateJwtToken(AlipayUser alipayUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtToken:SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, alipayUser.Id),
                    new Claim("bf:alibabaUserId", alipayUser.AlibabaUserId),
                    new Claim("bf:alipayUserId", alipayUser.AlipayUserId)
                }),
                Expires = DateTime.UtcNow.AddHours(_expiresIn),
                Issuer = _configuration.GetValue<string>("JwtToken:Issuer"),
                Audience = _configuration.GetValue<string>("JwtToken:Audience"),
                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string CreateJwtToken(WechatUser wechatUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtToken:SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, wechatUser.Id),
                    new Claim("bf:openId", wechatUser.OpenId)
                }),
                Expires = DateTime.UtcNow.AddHours(_expiresIn),
                Issuer = _configuration.GetValue<string>("JwtToken:Issuer"),
                Audience = _configuration.GetValue<string>("JwtToken:Audience"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
