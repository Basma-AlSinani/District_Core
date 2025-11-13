using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using ResidentsDatabase.Models;
//using Microsoft.EntityFrameworkCore.Tools;

namespace ResidentsDatabase
{
    public class ResidentsDbContext : DbContext
    {
        public DbSet<Resident> Residents { get; set; }   
        public ResidentsDbContext(DbContextOptions<ResidentsDbContext> options) : base(options) { }
    }
}
