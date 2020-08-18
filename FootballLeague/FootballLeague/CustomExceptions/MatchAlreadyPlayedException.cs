using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.CustomExceptions
{
    public class MatchAlreadyPlayedException : Exception
    {
        public MatchAlreadyPlayedException(string message) : base(message)
        {

        }
    }
}
