using System.Net.Http.Headers;
using System.Text;

namespace Crime.Middleware
{
    public class Authentication
    {
        private readonly RequestDelegate _next;
        public Authentication(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for PublicReports and Auth endpoints
            var path = context.Request.Path;
            if (path.StartsWithSegments("/api/PublicReports") || path.StartsWithSegments("/api/Auth"))
            {
                await _next(context);
                return;
            }

            // Check for the presence of the Authorization header
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Authorization header missing");
                return;
            }

            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            // Simple hardcoded check for demonstration purposes
            if (username != "admin" || password != "admin123")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Username or Password");
                return;
            }
            // If authentication is successful, proceed to the next middleware

            await _next(context);
        }
    }
}
