using BookingOfflineApp.Repositories.SqlServer;
using BookingOfflineApp.Web.Configurations;
using BookingOfflineApp.Web.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookingOfflineApp.Web
{
    public class Startup
    {
        private static IConfiguration Configuration { set; get; }
        private static IConfigurationRefresher ConfigurationRefresher { set; get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            EnsureLoadConfig();

            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<BODBContext>(options =>
                options.UseSqlServer(Configuration.GetValue<string>("BODatabase")));

            services.AddSwaggerUI();
            services.AddJwtAutentication(Configuration);

            services.AddBSSevices();
            services.AddDASevices();
            services.AddCommonSevices(Configuration);

            services.AddSingleton(Configuration);

            if (string.Equals(Configuration.GetValue<string>("Migration"), "on", StringComparison.OrdinalIgnoreCase))
            {
                services.BuildServiceProvider().GetService<BODBContext>().Database.Migrate();
            }
        }
        private void EnsureLoadConfig()
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)
                    .Select(KeyFilter.Any, LabelFilter.Null)
                    .Select(KeyFilter.Any, "prod")
                         //stop refresher if we use free tier
                         /*.ConfigureRefresh(refreshOptions =>
                              refreshOptions.Register("TestApp:Settings:Message")
                                            .SetCacheExpiration(TimeSpan.FromSeconds(60))
                          )*/;
                ConfigurationRefresher = options.GetRefresher();
            });

            Configuration = configBuilder.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
#if DEBUG
            app.UseMiddleware<FakeTokenMiddleware>();
#endif
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(option => option
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
            //app.UseMiddleware<CustomMiddleware>();
            app.UseJwtAuthenticaton();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
