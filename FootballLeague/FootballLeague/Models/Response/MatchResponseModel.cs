namespace FootballLeague.Models.Response
{
    /// <summary>
    /// This response model for the match.
    /// </summary>
    public class MatchResponseModel
    {
        /// <summary>
        /// Gets or sets the match unique identifier.
        /// </summary>
        /// <value>
        /// The match unique identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the home team name.
        /// </summary>
        /// <value>
        /// The home team name.
        /// </value>
        public string HomeTeamName { get; set; }

        /// <summary>
        /// Gets or sets the away team name.
        /// </summary>
        /// <value>
        /// The away team name.
        /// </value>
        public string AwayTeamName { get; set; }

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
        /// Gets or sets the league name.
        /// </summary>
        /// <value>
        /// The league name.
        /// </value>
        public string LeagueName { get; set; }
    }
}
