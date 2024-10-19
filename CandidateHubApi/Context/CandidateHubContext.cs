using Microsoft.EntityFrameworkCore;

namespace CandidateHubApi.Context
{

    public class CandidateHubContext : DbContext
    {
        public CandidateHubContext(DbContextOptions<CandidateHubContext> options) : base(options) { }
        public DbSet<Candidate> Candidates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
