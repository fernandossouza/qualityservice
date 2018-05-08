using Microsoft.EntityFrameworkCore;
using qualityservice.Model;

namespace qualityservice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // ...
        }

        public DbSet<ProductionOrderQuality> ProductionOrderQualities{get;set;}
        public DbSet<Analysis> Analyses{get;set;}
        public DbSet<AnalysisComp> AnalysisComps{get;set;}
        public DbSet<MessageCalculates> MessagesCalculates{get;set;}
        
    }
}