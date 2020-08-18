using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data
{
    /// <summary>
    /// Entity model for Team table
    /// </summary>
    public class Team   
    {
        /// <summary>
        /// Gets or sets the team unique identifier.
        /// </summary>
        /// <value>
        /// The team unique identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the team name.
        /// </summary>
        /// <value>
        /// The team name.
        /// </value>
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the team points.
        /// </summary>
        /// <value>
        /// The team points.
        /// </value>
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets the team scored goals.
        /// </summary>
        /// <value>
        /// The team scored goals.
        public int GoalsScored { get; set; }

        /// <summary>
        /// Gets or sets the team conceded goals.
        /// </summary>
        /// <value>
        /// The team conceded goals.
        public int GoalsConceded { get; set; }

        /// <summary>
        /// Gets or sets the team league id.
        /// </summary>
        /// <value>
        /// The team league id.
        public int? LeagueId { get; set; }

        public ICollection<Match> HomeMatches { get; set; }
        public ICollection<Match> AwayMatches { get; set; }
        public League League { get; set; }
    }
}
