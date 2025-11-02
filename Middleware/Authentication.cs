using System.Net.Http.Headers;
using System.Text;

namespace Crime.Middleware
{
    public class Authentication
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        public Authentication(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for PublicReports and Auth endpoints
            var header = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(header))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing Authorization header");
                return;
            }

            var encoded = header.Replace("Basic ", "");
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var parts = decoded.Split(':');
            var username = parts[0];
            var password = parts[1];
            // Retrieve valid credentials from configuration
            var validUser = _config["Authentication:Username"];
            var validPass = _config["Authentication:Password"];


            // Simple hardcoded check for demonstration purposes
            if (username != validUser || password != validPass)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid credentials");
                return;
            }

            // If authentication is successful, proceed to the next middleware

            await _next(context);
        }
    }
}
