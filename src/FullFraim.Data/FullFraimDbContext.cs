using FullFraim.Data.Models;
using FullFraim.Data.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FullFraim.Data
{
    public class FullFraimDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public FullFraimDbContext(DbContextOptions<FullFraimDbContext> options)
            : base(options) { }

        public DbSet<Contest> Contests { get; set; }
        public DbSet<ContestCategory> ContestCategories { get; set; }
        public DbSet<ContestType> ContestTypes { get; set; }
        public DbSet<Phase> Phases { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<PhotoReview> PhotoReviews { get; set; }
        public DbSet<ContestPhase> ContestPhases { get; set; }
        public DbSet<JuryContest> JuryContests { get; set; }
        public DbSet<ParticipantContest> ParticipantContests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Seed();

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
