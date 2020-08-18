using FootballLeague.Bll.Interfaces;
using FootballLeague.Dal.Interfaces;
using FootballLeague.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Bll
{
    public class LeagueEngine : ILeagueEngine
    {
        private readonly ILeagueStore _leagueStore;

        public LeagueEngine(ILeagueStore leagueStore)
        {
            _leagueStore = leagueStore ?? throw new ArgumentNullException(nameof(leagueStore));
        }

        public async Task<int> CreateLeagueAsync(LeagueRequestModel leagueModel, CancellationToken cancellationToken)
        {
            return await _leagueStore.CreateLeagueAsync(leagueModel, cancellationToken);
        }
    }
}
