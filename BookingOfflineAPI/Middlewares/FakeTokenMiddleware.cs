using BookingOfflineApp.Common;
using BookingOfflineApp.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJlNWIxYmRmMi1hNzk5LTQ4M2YtODcxNy1jYzQzODkyOTU0MDQiLCJiZjphbGliYWJhVXNlcklkIjoiMjA4ODAwMjYzODM4MDg0MCIsImJmOmFsaXBheVVzZXJJZCI6IjIwODgxMDY5NTIxNzYxNTI5NzMwNTE0ODAyMDE4NDg0IiwibmJmIjoxNTg1NzUxNDUwLCJleHAiOjE1ODYzNTYyNTAsImlhdCI6MTU4NTc1MTQ1MH0.-uUq2qBJtgiAPrAJpupNTBSsv6K0CwD4kTU7rEfcCXQ";
            httpContext.Request.Headers.Add("Authorization", "Bearer " + token);
            await _next(httpContext);

        }
    }
}
