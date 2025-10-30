using Crime;
using Crime.Repositories;
using Crime.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.AccessControl;
using System.Security.Policy;


namespace Crime
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CrimeDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // register repositories and services
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

            builder.Services.AddScoped<IEvidenceAuditLogsRepo, EvidenceAuditLogsRepo>();
            builder.Services.AddScoped<IEvidenceAuditLogsService, EvidenceAuditLogsService>();

            builder.Services.AddScoped<ICaseReportRepo, CaseReportRepo>();
            builder.Services.AddScoped<ICaseReportService, CaseReportService>();

            builder.Services.AddScoped<ICaseAssigneesService, CaseAssigneesService>();
            builder.Services.AddScoped<ICaseAssigneesRepo, CaseAssigneesRepo>();

            builder.Services.AddScoped<ICaseCommentRepo, CaseCommentRepo>();
            builder.Services.AddScoped<ICaseCommentService, CaseCommentService>();

            builder.Services.AddAutoMapper(typeof(Mapping.Mapping));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
