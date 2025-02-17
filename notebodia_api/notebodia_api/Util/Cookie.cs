
namespace notebodia_api.Util
{
    public static class CookieUtil
    {
        public static void SetCookie(HttpContext httpContext, string name, string value, int day)
        {
            if (httpContext == null) return;
            httpContext.Response.Cookies.Append(name, value, new CookieOptions
            {
                HttpOnly = true,  // Prevents JavaScript access (security)
                Secure = true,    // Ensures the cookie is sent only over HTTPS (enable in production)
                SameSite = SameSiteMode.Strict, // Protects against CSRF attacks
                Expires = DateTime.UtcNow.AddDays(day)
            });
        }

        public static string? GetCookie(HttpContext httpContext, string name)
        {
            return httpContext?.Request.Cookies[name];
        }

        public static void DeleteCookie(HttpContext httpContext, string name)
        {
            if (httpContext == null) return;
            httpContext.Response.Cookies.Delete(name);
        }
    }
}