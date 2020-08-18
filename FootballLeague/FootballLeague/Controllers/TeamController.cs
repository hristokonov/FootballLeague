using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    /// <summary>
    /// Class responsible for team operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public TeamController(ITeamService teamService, ILogger<TeamController> logger, IMemoryCache cache)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Create a team
        /// </summary>
        /// <returns>The task.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateTeamAsync(TeamRequestModel teamModel, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateTeamAsync.");

            var teamId = await _teamService.CreateTeamAsync(teamModel, cancellationToken);

            return new ObjectResult(teamId) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Delete a team
        /// </summary>
        /// <returns>The task.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeamAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to DeleteTeamAsync.");

            await _teamService.DeleteTeamAsync(id, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Get a team
        /// </summary>
        /// <returns>The task.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseModel>> GetTeamDetailsAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetTeamAsync.");

            var teamDetails = await CacheDetailsResponse(id, cancellationToken);

            return teamDetails;
        }

        private async Task<TeamResponseModel> CacheDetailsResponse(int id, CancellationToken cancellationToken)
        {
            var cashedResponse = await _cache.GetOrCreateAsync<TeamResponseModel>("TeamResponse", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(2);
                var response = await _teamService.GetTeamDetailsAsync(id, cancellationToken);
                return response;
            });

            return cashedResponse;
        }
    }
}
