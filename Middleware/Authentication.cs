using Crime.Repositories;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Crime.Middleware
{
    public class Authentication
    {
        private readonly RequestDelegate _next;

        public Authentication(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUsersRepo userRepo)
        {
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (path.Contains("/api/citizenreport") || path.Contains("/api/public")))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing Authorization header");
                return;
            }

            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter ?? ""));
            var parts = credentials.Split(':', 2);
            if (parts.Length != 2)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Authorization header");
                return;
            }

            var username = parts[0];
            var password = parts[1];

            var user = await userRepo.GetByUsernameAsync(username);
            if (user == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid username or password");
                return;
            }

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = Convert.ToBase64String(hashBytes);

            if (user.PasswordHash != hashedPassword)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid username or password");
                return;
            }

            context.Items["User"] = user;
            await _next(context);
        }
    }
}
