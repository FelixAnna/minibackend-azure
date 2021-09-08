using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookingOfflineApp.Web.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static void AddJwtAutentication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = Encoding.ASCII.GetBytes(configuration["JwtToken:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetValue<string>("JwtToken:Issuer"),
                        ValidAudience = configuration.GetValue<string>("JwtToken:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

        }

        public static void UseJwtAuthenticaton(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();
        }
    }
}
