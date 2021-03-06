﻿using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models.Request
{
    /// <summary>
    /// Request model for the League
    /// </summary>
    public class LeagueRequestModel
    {
        /// <summary>
        /// Gets or sets the league name.
        /// </summary>
        /// <value>
        /// The league name.
        /// </value>
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
