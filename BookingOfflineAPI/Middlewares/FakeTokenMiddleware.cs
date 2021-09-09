using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookingOfflineApp.Web.Middlewares
{
    public class FakeTokenMiddleware
    {
        private readonly RequestDelegate _next;
        public FakeTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJ5dWZlbGl4IiwiYmY6YWxpYmFiYVVzZXJJZCI6IjEyMzRhIiwiYmY6YWxpcGF5VXNlcklkIjoiMTIzNDU2YWJjIiwibmJmIjoxNjMxMTYwNzc4LCJleHAiOjE2MzE0MTk5NzgsImlhdCI6MTYzMTE2MDc3OH0.J7d9CFSPf-ek5UWMkWD_ecy_oHiVyUj-d7hfKRm1V-U";
            httpContext.Request.Headers.Add("Authorization", "Bearer " + token);
            await _next(httpContext);

        }
    }
}
