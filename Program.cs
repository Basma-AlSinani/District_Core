using Crime;
using Crime.Repositories;
using Crime.Services;

using Microsoft.EntityFrameworkCore;
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
            builder.Services.AddScoped<IEvidenceRepo, EvidenceRepo>();
            builder.Services.AddScoped<IEvidenceService, EvidenceService>();
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
