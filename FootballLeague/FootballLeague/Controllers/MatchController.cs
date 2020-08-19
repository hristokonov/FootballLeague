using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly ILogger _logger;

        public MatchController(IMatchService matchService, ILogger<TeamController> logger)
        {
            _matchService = matchService;
            _logger = logger;
        }

        /// <summary>
        /// Create a match
        /// </summary>
        /// <returns>The task.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateMatch(MatchRequestModel matchModel, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateMatch.");

            var matchId = await _matchService.CreateMatchAsync(matchModel, cancellationToken);

            return new ObjectResult(matchId) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Play a match
        /// </summary>
        /// <returns>The task.</returns>
        [HttpPut]
        public async Task<ActionResult> PlayMatch(MatchRequestModel matchModel, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to PlayMatch.");

            await _matchService.PlayMatchAsync(matchModel, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Get all matches
        /// </summary>
        /// <returns>The task.</returns>
        [HttpGet]
        public async Task<ActionResult<ICollection<MatchResponseModel>>> GetAllMatches(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetAllMatches.");

            var matches = await _matchService.GetAllMatchesAsync(cancellationToken);

            return Ok(matches);
        }

        /// <summary>
        /// Get all matches of a team
        /// </summary>
        /// <returns>The task.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MatchResponseModel>>> GetAllTeamMatches(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetAllTeamMatches.");

            var matches = await _matchService.GetAllTeamMatchesAsync(id, cancellationToken);

            return Ok(matches);
        }

        /// <summary>
        /// Delete a match
        /// </summary>
        /// <returns>The task.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to DeleteMatch.");

            await _matchService.DeleteMatchAsync(id, cancellationToken);

            return Ok();
        }
    }
}
