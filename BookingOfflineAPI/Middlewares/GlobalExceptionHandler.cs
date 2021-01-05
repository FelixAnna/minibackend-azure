using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BookingOfflineApp.Web.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                httpContext.Response.StatusCode = 500;
                await HandleExceptionAsync(httpContext, 500, ex.Message);
            }
            finally
            {
                var statusCode = httpContext.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                {
                    msg = "未授权";
                }
                else if (statusCode == 403)
                {
                    msg = "权限不足";
                }
                else if (statusCode == 404)
                {
                    msg = "未找到资源";
                }
                else if (statusCode > 500)
                {
                    msg = "服务器错误";
                }
                else if (statusCode == 400)
                {
                    msg = "客户端错误";
                }
                else if (statusCode != 200)
                {
                    msg = "未知错误";
                }

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(httpContext, statusCode, msg);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var data = new { code = statusCode.ToString(), is_success = false, msg = msg };
            var result = JsonConvert.SerializeObject(new { data = data });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
}
