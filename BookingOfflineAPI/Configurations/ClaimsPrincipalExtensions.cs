using System.Security.Claims;

namespace BookingOfflineApp.Web.Configurations
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAlipayUser(this ClaimsPrincipal claims)
        {
            if (claims.FindFirst("bf:alibabaUserId") != null && !string.IsNullOrEmpty(claims.FindFirstValue("bf:alibabaUserId")))
            {
                return true;
            }

            return false;
        }
    }
}
