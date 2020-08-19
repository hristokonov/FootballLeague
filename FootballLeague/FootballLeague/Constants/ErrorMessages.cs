namespace FootballLeague.Constants
{
    public static class ErrorMessages
    {
        public const string MatchNotFound = "Match with this id {0} doesn't exist";
        public const string MatchIsPlayed = "Match with this id {0} between {1} and {2} is already played";
        public const string TeamNotFound = "Team with this id {0} doesn't exist";
        public const string TeamNotInLeague = "Team with this id {0} doesn't exist in this league with id {1}";
        public const string TeamExists = "Team with this name {0} already exist";
        public const string LeagueNotFound = "League with this id {0} doesn't exist";
        public const string LeagueExists = "League with this name {0} already exist";
    }
}
