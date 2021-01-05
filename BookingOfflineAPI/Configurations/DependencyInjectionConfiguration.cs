using BookingOfflineApp.Common;
using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using BookingOfflineApp.Repositories.SqlServer; //or Sqlite
using BookingOfflineApp.Services;
using BookingOfflineApp.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingOfflineApp.Web.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddBSSevices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddDASevices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository<AlipayUser>, AlipayUserRepository>();
            services.AddScoped<IUserRepository<WechatUser>, WechatUserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<BODBContext>();
        }

        public static void AddCommonSevices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAlipayService, AlipayService>(provider => new AlipayService(configuration["Alipay:AppId"], configuration["Alipay:PrivateKey"], configuration["Alipay:PublicKey"]));
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();

            services.AddScoped<IWechatService, WechatService>(provider => new WechatService(configuration["Wechat:AppId"], configuration["Wechat:Secret"]));
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        }
    }
}
