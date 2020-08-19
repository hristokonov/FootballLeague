using System;

namespace FootballLeague.CustomExceptions
{
    public class MatchAlreadyPlayedException : Exception
    {
        public MatchAlreadyPlayedException()
        {

        }
        public MatchAlreadyPlayedException(string message) : base(message)
        {

        }
    }
}
