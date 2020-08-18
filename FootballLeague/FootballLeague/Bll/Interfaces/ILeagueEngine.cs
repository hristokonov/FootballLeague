using FootballLeague.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Bll.Interfaces
{
    public interface ILeagueEngine
    {
        /// <summary>
        /// Create a league.
        /// </summary>
        /// <param name="leagueModel">League request model.</param>
        /// <returns>League id.</returns>
        Task<int> CreateLeagueAsync(LeagueRequestModel leagueModel, CancellationToken cancellationToken);
    }
}
