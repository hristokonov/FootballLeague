using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FootballLeague.Data
{
    public class Team   
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string City { get; set; }

        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConcede { get; set; }
        public int GoalDifference { get; set; }

        public int? LeagueId { get; set; }
        public League League { get; set; }
      
        public ICollection<Match> Matches { get; set; }
      //  public ICollection<Match> AwayMatches { get; set; }
    }
}
