using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
            _matchService = matchService ?? throw new ArgumentNullException(nameof(matchService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Create a match
        /// </summary>
        /// <returns>The task.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateMatchAsync(MatchRequestModel matchModel, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateTeamAsync.");

            var mathcId = await _matchService.CreateMatchAsync(matchModel, cancellationToken);

            return new ObjectResult(mathcId) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Play a match
        /// </summary>
        /// <returns>The task.</returns>
        [HttpPut]
        public async Task<ActionResult> PlayMatchAsync(MatchRequestModel matchModel, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to PlayMatchAsync.");

            await _matchService.PlayMatchAsync(matchModel, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Get all matches
        /// </summary>
        /// <returns>The task.</returns>
        [HttpGet]
        public async Task<ActionResult<ICollection<MatchResponseModel>>> GetAllMatchesAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetAllMatchesAsync.");

            var matches = await _matchService.GetAllMatchesAsync(cancellationToken);

            return new ObjectResult(matches) { StatusCode = StatusCodes.Status200OK };
        }

        /// <summary>
        /// Get all matches of a team
        /// </summary>
        /// <returns>The task.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MatchResponseModel>>> GetAllTeamMatchesAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetAllTeamMatchesAsync.");

            var matches = await _matchService.GetAllTeamMatchesAsync(id, cancellationToken);

            return new ObjectResult(matches) { StatusCode = StatusCodes.Status200OK };
        }

        /// <summary>
        /// Delete a match
        /// </summary>
        /// <returns>The task.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatchAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to DeleteTeamAsync.");

            await _matchService.DeleteMatchAsync(id, cancellationToken);

            return Ok();
        }
    }
}
