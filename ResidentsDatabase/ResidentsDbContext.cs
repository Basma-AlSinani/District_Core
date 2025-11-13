using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
//using Microsoft.EntityFrameworkCore.Tools;

namespace ResidentsDatabase
{
    public class ResidentsDbContext : DbContext
    {
        public ResidentsDbContext(DbContextOptions<ResidentsDbContext> options) : base(options) { }
    }
}
