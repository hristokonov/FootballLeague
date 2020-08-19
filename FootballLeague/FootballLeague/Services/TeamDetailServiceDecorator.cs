using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
    public class TeamDetailServiceDecorator : ITeamDetailService
    {
        private readonly TeamDetailService _teamDetailService;
        private readonly IMemoryCache _cache;
        private const int ExpirationInMinutes = 2;

        public TeamDetailServiceDecorator(TeamDetailService teamDetailService, IMemoryCache cache)
        {
            _teamDetailService = teamDetailService;
            _cache = cache;
        }
        public async Task<TeamResponseModel> GetTeamDetailsAsync(int id, CancellationToken cancellationToken)
        {
            var cashedData = await _cache.GetOrCreateAsync(id, async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(ExpirationInMinutes);
                var result = await _teamDetailService.GetTeamDetailsAsync(id, cancellationToken);
                return result;
            });

            return cashedData;
        }
    }
}
