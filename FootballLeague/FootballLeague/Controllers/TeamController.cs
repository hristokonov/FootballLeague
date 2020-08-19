using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public TeamController(ITeamService teamService, ILogger<TeamController> logger)
        {
            _teamService = teamService;
            _logger = logger;
        }

        /// <summary>
        /// Create a team
        /// </summary>
        /// <returns>The task.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateTeam(TeamRequestModel teamModel, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateTeam.");

            var teamId = await _teamService.CreateTeamAsync(teamModel, cancellationToken);

            return new ObjectResult(teamId) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Delete a team
        /// </summary>
        /// <returns>The task.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeam(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to DeleteTeam.");

            await _teamService.DeleteTeamAsync(id, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Get a team
        /// </summary>
        /// <returns>The task.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseModel>> GetTeamDetails(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetTeamDetails.");

            var teamDetails = await _teamService.GetTeamDetailsAsync(id, cancellationToken);

            return Ok(teamDetails);
        }
    }
}
