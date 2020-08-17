using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FootballLeague.Data
{
    public class FootballLeagueDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<League> Leagues { get; set; }

        public IConfiguration Configuration { get; }

        public FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options)
           : base(options)
        {

        }

        public FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options, IConfiguration configuration)
         : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionHristo"));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //LoadJson(builder);

            builder.Entity<Match>()
                         .HasOne(m => m.AwayTeam)
                         .WithMany(t => t.AwayMatches)
                         .HasForeignKey(m => m.AwayTeamId)
                           .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

            builder.Entity<Match>()
                        .HasOne(m => m.HomeTeam)
                        .WithMany(t => t.HomeMatches)
                        .HasForeignKey(m => m.HomeTeamId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

            base.OnModelCreating(builder);
        }

    }
}
