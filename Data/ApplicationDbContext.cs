using Microsoft.EntityFrameworkCore;
using qualityservice.Model;

namespace qualityservice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ProductionOrderQuality> ProductionOrderQualities{get;set;}
        public DbSet<Analysis> Analyses{get;set;}
        
    }
}