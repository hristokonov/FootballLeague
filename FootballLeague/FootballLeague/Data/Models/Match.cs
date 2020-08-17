using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballLeague.Data
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public int HomeTeamId { get; set; }
        [ForeignKey("HomeTeamId")]
        [InverseProperty("HomeMatches")]
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        [ForeignKey("AwayTeamId")]
        [InverseProperty("AwayMatches")]
        public Team AwayTeam { get; set; }

        public bool IsMatchPlayed { get; set; }

        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public DateTime Date { get; set; }

        public League League { get; set; }
        public int LeagueId { get; set; }
    }
}
