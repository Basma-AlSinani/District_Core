using Crime;
using Crime.Mapping;
using Crime.Models;
using Crime.Repositories;
using Crime.Services;
using Crime.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Environment =====
builder.Environment.EnvironmentName = "Development";

// ===== Services =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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
});

// ===== DbContext =====
builder.Services.AddDbContext<CrimeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== Repositories & Services =====
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

builder.Services.AddAutoMapper(typeof(Mapping));

// ===== Authentication =====
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

// ===== Swagger =====
app.UseSwagger();
app.UseSwaggerUI();

// ===== Middleware =====
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ===== Map Controllers =====
app.MapControllers();

// ===== Seed Default Admin =====
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
