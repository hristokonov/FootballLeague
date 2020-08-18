using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models.Request
{
    /// <summary>
    /// Request model for the Team
    /// </summary>
    public class TeamRequestModel
    {
        /// <summary>
        /// Gets or sets the team name.
        /// </summary>
        /// <value>
        /// The team name.
        /// </value>
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the team league id.
        /// </summary>
        /// <value>
        /// The team league id.
        public int LeagueId { get; set; }
    }
}
