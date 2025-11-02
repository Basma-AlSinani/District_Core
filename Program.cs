using Crime;
using Crime.Mapping;
using Crime.Models;
using Crime.Repositories;
using Crime.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
