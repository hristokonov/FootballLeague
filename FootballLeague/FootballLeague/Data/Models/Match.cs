using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public int? HomeTeamId { get; set; }

        public int? AwayTeamId { get; set; }

        public bool IsMatchPlayed { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public DateTime Date { get; set; }

        public int LeagueId { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public League League { get; set; }
    }
}
