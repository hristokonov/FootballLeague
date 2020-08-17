using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data
{
    public class League
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public ICollection<Team> Teams { get; set; }

        public ICollection<Match> Matches { get; set; }
        // public virtual ICollection<LeagueMatches> LeagueMatches { get; set; }
    }
}
