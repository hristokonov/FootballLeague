using FootballLeague.Bll.Interfaces;
using FootballLeague.Models.Request;
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
    public class LeagueController : ControllerBase
    {
        private readonly ILeagueEngine _leagueEngine;
        private readonly ILogger _logger;

        public LeagueController(ILeagueEngine leagueEngine, ILogger<LeagueController> logger)
        {
            _leagueEngine = leagueEngine ?? throw new ArgumentNullException(nameof(leagueEngine));
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

            var leagueId = await _leagueEngine.CreateLeagueAsync(leagueModel, cancellationToken);

            return new ObjectResult(leagueId) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
