using FootballLeague.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Dal.Interfaces
{
    public interface ILeagueStore
    {
        /// <summary>
        /// Create a league.
        /// </summary>
        /// <param name="leagueModel">League request model.</param>
        /// <returns>League id.</returns>
        Task<int> CreateLeagueAsync(LeagueRequestModel league, CancellationToken cancellationToken);
    }
}
