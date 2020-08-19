using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Models
{
    /// <summary>
    /// Entity model for League table
    /// </summary>
    public class League
    {
        /// <summary>
        /// Gets or sets the league unique identifier.
        /// </summary>
        /// <value>
        /// The league unique identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the league name.
        /// </summary>
        /// <value>
        /// The league name.
        /// </value>
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public ICollection<Team> Teams { get; set; }

        public ICollection<Match> Matches { get; set; }
    }
}
