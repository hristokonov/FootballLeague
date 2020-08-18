using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;

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

        private void LoadJson(ModelBuilder builder)
        {
            if (File.Exists(@"..\FootballLeague\Data\JsonFiles\Leagues.json")
                && File.Exists(@"..\FootballLeague\Data\JsonFiles\Teams.json")
                && File.Exists(@"..\FootballLeague\Data\JsonFiles\Matches.json")
              )
            {
                var leagues = JsonConvert.DeserializeObject<League[]>
                   (File.ReadAllText(@"..\FootballLeague\Data\JsonFiles\Leagues.json"));
                var teams = JsonConvert.DeserializeObject<Team[]>
                    (File.ReadAllText(@"..\FootballLeague\Data\JsonFiles\Teams.json"));
                var matches = JsonConvert.DeserializeObject<Match[]>
                    (File.ReadAllText(@"..\FootballLeague\Data\JsonFiles\Matches.json"));

                builder.Entity<League>().HasData(leagues);
                builder.Entity<Team>().HasData(teams);
                builder.Entity<Match>().HasData(matches);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            LoadJson(builder);

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

            builder.Entity<League>()
                        .HasIndex(l => l.Name)
                        .IsUnique();

            builder.Entity<Team>()
                        .HasIndex(t => t.Name)
                        .IsUnique();

            base.OnModelCreating(builder);
        }

    }
}
