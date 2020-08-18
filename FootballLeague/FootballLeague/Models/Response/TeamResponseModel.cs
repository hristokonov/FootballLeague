namespace FootballLeague.Models.Response
{
    public class TeamResponseModel
    {
        /// <summary>
        /// Gets or sets the team unique identifier.
        /// </summary>
        /// <value>
        /// The team unique identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the team name.
        /// </summary>
        /// <value>
        /// The team name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the played matches.
        /// </summary>
        /// <value>
        /// The played matches.
        /// </value>
        public int MatchesPlayed { get; set; }

        /// <summary>
        /// Gets or sets the scored goals by the team.
        /// </summary>
        /// <value>
        /// The scored goals from the team.
        /// </value>
        public int GoalsScored { get; set; }

        /// <summary>
        /// Gets or sets the conceded goals by the team.
        /// </summary>
        /// <value>
        /// The conceded goals from the team.
        /// </value>
        public int GoalsConceded { get; set; }

        /// <summary>
        /// Gets or sets the team goal difference.
        /// </summary>
        /// <value>
        /// The team goal difference.
        /// </value>
        public int GoalDifference { get; set; }

        /// <summary>
        /// Gets or sets the team points.
        /// </summary>
        /// <value>
        /// The team points.
        /// </value>
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets the league name.
        /// </summary>
        /// <value>
        /// The league name.
        /// </value>
        public string LeagueName { get; set; }

        /// <summary>
        /// Gets or sets the team league position.
        /// </summary>
        /// <value>
        /// The team league position.
        /// </value>
        public int Position { get; set; }
    }
}
