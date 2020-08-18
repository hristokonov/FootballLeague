using System.Collections.Generic;

namespace FootballLeague.Models.Response
{
    /// </summary>
    /// This response model is used to get the information about a league table.
    /// </summary>
    public class LeagueResponseModel
    {
        /// <summary>
        /// Gets or sets the league name.
        /// </summary>
        /// <value>
        /// The league name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the teams in the league.
        /// </summary>
        /// <value>
        /// The teams in the league.
        /// </value>
        public ICollection<TeamResponseModel> Teams { get; set; }
    }
}
