using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Reflection.Emit;

namespace WorkoutTracker.Models
{
    public class PgsqlDbContext : DbContext
    {
        public PgsqlDbContext(DbContextOptions<PgsqlDbContext> options) : base(options) { }


        public DbSet<Exercise> Exercise => Set<Exercise>();
        public DbSet<User> User => Set<User>();
        public DbSet<TemplateHeader> TemplateHeader => Set<TemplateHeader>();
        public DbSet<TemplateSpecification> TemplateSpecification => Set<TemplateSpecification>();
        public DbSet<WorkoutHeader> WorkoutHeader => Set<WorkoutHeader>();
        public DbSet<WorkoutSpecification> WorkoutSpecification => Set<WorkoutSpecification>();
        public DbSet<WorkoutSet> WorkoutSet => Set<WorkoutSet>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Exercise");
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("host=192.168.0.135;port=5432;database=Sivuha_TMS;user id=postgres;password=12345;pooling=false;Connection LifeTime=0;",
                o => o.SetPostgresVersion(9,1));
        }
    }
}
