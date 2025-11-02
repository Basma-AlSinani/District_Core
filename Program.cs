using Crime;
using Crime.Mapping;
using Crime.Models;
using Crime.Repositories;
using Crime.Services;
using Crime.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Crime.Helpers;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// ======================
// 🌍 Environment
// ======================
builder.Environment.EnvironmentName = "Development";

// ======================
// 📦 Services
// ======================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
    });

builder.Services.AddEndpointsApiExplorer();

// ======================
// 🧾 Swagger Configuration
// ======================
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "basic",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Basic Authentication header"
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
            new string[] { }
        }
    });

    
    c.SchemaFilter<EnumSchemaFilter>();
});



// ======================
// 🗄️ Database Context
// ======================
builder.Services.AddDbContext<CrimeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ======================
// 🧩 Dependency Injection
// ======================
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

builder.Services.AddScoped<ICaseAssigneesRepo, CaseAssigneesRepo>();
builder.Services.AddScoped<ICaseAssigneesService, CaseAssigneesService>();

builder.Services.AddScoped<ICaseCommentRepo, CaseCommentRepo>();
builder.Services.AddScoped<ICaseCommentService, CaseCommentService>();

builder.Services.AddAutoMapper(typeof(Mapping));

// ======================
// 🔐 Authentication
// ======================
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

// ======================
// 🚀 Middleware
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
// 👑 Seed Default Admin
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
// ▶️ Run App
// ======================
app.Run();
