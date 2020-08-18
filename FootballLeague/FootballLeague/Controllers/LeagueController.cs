using FootballLeague.Models.Request;
using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly ILogger _logger;

        public LeagueController(ILeagueService leagueService, ILogger<LeagueController> logger)
        {
            _leagueService = leagueService ?? throw new ArgumentNullException(nameof(leagueService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Create a league
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateLeagueAsync(LeagueRequestModel leagueModel, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateLeagueAsync.");

            var leagueId = await _leagueService.CreateLeagueAsync(leagueModel, cancellationToken);

            return new ObjectResult(leagueId) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Get league table
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<LeagueRequestModel>> GetLeagueTableAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetLeagueTableAsync.");

            var leagueModel = await _leagueService.GetLeagueTableAsync(id, cancellationToken);

            return new ObjectResult(leagueModel) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
