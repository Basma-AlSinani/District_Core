using CrimeManagment;
using CrimeManagment.Mapping;
using CrimeManagment.Models;
using CrimeManagment.Repositories;
using CrimeManagment.Services;
using CrimeManagment.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ======================
//  Environment
// ======================
builder.Environment.EnvironmentName = "Development";

// ======================
//  Services
// ======================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();

// ======================
//  Swagger Configuration
// ======================
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    c.SchemaFilter<EnumSchemaFilter>();
});

// ======================
//  Database Context
// ======================
builder.Services.AddDbContext<CrimeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ======================
//  Dependency Injection
// ======================
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEvidenceRepo, EvidenceRepo>();
builder.Services.AddScoped<IEvidenceService, EvidenceService>();
builder.Services.AddScoped<ICasesRepo, CasesRepo>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IParticipantsRepo, ParticipantsRepo>();
builder.Services.AddScoped<IParticipantService, ParticipantService> ();
builder.Services.AddScoped<IUsersRepo, UsersRepo>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ICaseParticipantsRepo, CaseParticipantsRepo>();
builder.Services.AddScoped<ICaseParticipantService, CaseParticipantService>();
builder.Services.AddScoped<IEvidenceAuditLogsRepo, EvidenceAuditLogsRepo>();
builder.Services.AddScoped<IEvidenceAuditLogsService, EvidenceAuditLogsService>();
builder.Services.AddScoped<ICrimeReportsRepository, CrimeReportsRepo>();
builder.Services.AddScoped<ICrimeReportsService, CrimeReportsService>();
builder.Services.AddScoped<ICaseAssigneesRepo, CaseAssigneesRepo>();
builder.Services.AddScoped<ICaseAssigneesService, CaseAssigneesService>();
builder.Services.AddScoped<ICaseCommentRepo, CaseCommentRepo>();
builder.Services.AddScoped<ICaseCommentService, CaseCommentService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);
builder.Services.AddAutoMapper(typeof(Mapping));

// ======================
//  JWT Authentication
// ======================
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader))
            {
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    context.Token = authHeader.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    context.Token = authHeader.Trim();
                }
            }
            return Task.CompletedTask;
        },

        OnChallenge = context =>
        {
            // Prevents the default 401 response
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                Message = "You are not authorized. Please log in first."
            });

            return context.Response.WriteAsync(result);
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                message = "You do not have permission to access this resource."
            });

            return context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();

// ======================
//  Build App
// ======================
var app = builder.Build();

// ======================
//  Middleware
// ======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ======================
//  Seed Default Admin
// ======================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CrimeDbContext>();

    if (!db.Users.Any(u => u.Username == "admin"))
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes("Admin123!"));
        var hashedPassword = Convert.ToBase64String(hashBytes);

        var adminUser = new Users
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
        };

        db.Users.Add(adminUser);
        db.SaveChanges();
    }
}

// ======================
// ▶ Run App
// ======================
app.Run();
