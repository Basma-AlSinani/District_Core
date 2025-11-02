using Crime;
using Crime.Mapping;
using Crime.Models;
using Crime.Repositories;
using Crime.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "basic",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<CrimeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEvidenceRepo, EvidenceRepo>();
builder.Services.AddScoped<IEvidenceService, EvidenceService>();
builder.Services.AddScoped<ICasesRepo, CasesRepo>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IParticipantsRepo, ParticipantsRepo>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IUsersRepo, UsersRepo>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ICaseParticipantsRepo, CaseParticipantsRepo>();
builder.Services.AddScoped<ICaseParticipantService, CaseParticipantService>();
builder.Services.AddScoped<IEvidenceAuditLogsRepo, EvidenceAuditLogsRepo>();
builder.Services.AddScoped<IEvidenceAuditLogsService, EvidenceAuditLogsService>();
builder.Services.AddScoped<ICrimeReportsRepository, CrimeReportsRepo>();
builder.Services.AddScoped<ICrimeReportsServies, CrimeReportsServies>();
builder.Services.AddScoped<ICaseReportRepo, CaseReportRepo>();
builder.Services.AddScoped<ICaseReportService, CaseReportService>();
builder.Services.AddScoped<ICaseAssigneesService, CaseAssigneesService>();
builder.Services.AddScoped<ICaseAssigneesRepo, CaseAssigneesRepo>();
builder.Services.AddScoped<ICaseCommentRepo, CaseCommentRepo>();
builder.Services.AddScoped<ICaseCommentService, CaseCommentService>();
builder.Services.AddAutoMapper(typeof(Crime.Mapping.Mapping));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Use(async (context, next) =>
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            var credentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter ?? ""));
            var parts = credentials.Split(':', 2);
            if (parts.Length != 2 || parts[0] != "admin" || parts[1] != "Admin123!")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid credentials");
                return;
            }
        }
        await next();
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<Crime.Middleware.Authentication>();
app.UseAuthorization();
app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<CrimeDbContext>();
    if (!dbContext.Users.Any(u => u.Username == "admin"))
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Admin123!"));
        var hashedPassword = Convert.ToBase64String(hashBytes);
        dbContext.Users.Add(new Users
        {
            FirstName = "System",
            SecondName = "Admin",
            LastName = "Admin",
            FullName = "System Admin",
            Username = "admin",
            PasswordHash = hashedPassword,
            Email = "admin@crime.gov",
            Role = UserRole.Admin,
            ClearanceLevel = ClearanceLevel.High
        });
        dbContext.SaveChanges();
    }
}

app.Run();
