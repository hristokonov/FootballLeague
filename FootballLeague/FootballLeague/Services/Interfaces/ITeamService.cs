using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface ITeamService
    {
        /// <summary>
        /// Create a team.
        /// </summary>
        /// <param name="teamModel">Team request model.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>Team id.</returns>
        Task<int> CreateTeamAsync(TeamRequestModel teamModel, CancellationToken cancellationToken);

        /// <summary>
        /// Delete a team.
        /// </summary>
        /// <param name="teamId">Team id.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>Team id.</returns>
        Task DeleteTeamAsync(int teamId, CancellationToken cancellationToken);

        /// <summary>
        /// Get team details.
        /// </summary>
        /// <param name="teamId">The team id.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>TeamResponseModel</returns>
        Task<TeamResponseModel> GetTeamDetailsAsync(int teamId, CancellationToken cancellationToken);

        /// <summary>
        /// Checking if team is in league.
        /// </summary>
        /// <param name="teamId">The team id.</param>
        /// <param name="leagueId">The league id.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns></returns>
        Task CheckIfTeamExistInLeague(int teamId, int leagueId, CancellationToken cancellationToken);
    }
}
