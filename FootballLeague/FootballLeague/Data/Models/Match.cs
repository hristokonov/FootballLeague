using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Models
{
    /// <summary>
    /// Entity model for Match table
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Gets or sets the match unique identifier.
        /// </summary>
        /// <value>
        /// The match unique identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the home team unique identifier.
        /// </summary>
        /// <value>
        /// The home team unique identifier.
        /// </value>
        public int? HomeTeamId { get; set; }

        /// <summary>
        /// Gets or sets the away team unique identifier.
        /// </summary>
        /// <value>
        /// The away team unique identifier.
        /// </value>
        public int? AwayTeamId { get; set; }

        /// <summary>
        /// Gets or sets flag is the match played.
        /// </summary>
        /// <value>
        /// Is the match played flag.
        /// </value>
        public bool IsMatchPlayed { get; set; }

        /// <summary>
        /// Gets or sets the home team goals.
        /// </summary>
        /// <value>
        /// The home team goals.
        /// </value>
        public int HomeTeamGoals { get; set; }

        /// <summary>
        /// Gets or sets the away team goals.
        /// </summary>
        /// <value>
        /// The away team goals.
        /// </value>
        public int AwayTeamGoals { get; set; }

        /// <summary>
        /// Gets or sets the match league id.
        /// </summary>
        /// <value>
        /// The match league id.
        /// </value>
        public int LeagueId { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public League League { get; set; }
    }
}
