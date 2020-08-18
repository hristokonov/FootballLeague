using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface ILeagueService
    {
        /// <summary>
        /// Create a league.
        /// </summary>
        /// <param name="leagueModel">League request model.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>League id.</returns>
        Task<int> CreateLeagueAsync(LeagueRequestModel leagueModel, CancellationToken cancellationToken);

        /// <summary>
        /// Check if league exist by id
        /// </summary>
        /// <param name="leagueId">League id.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        Task CheckIfLeagueExistById(int leagueId, CancellationToken cancellationToken);

        /// <summary>
        /// Get league table
        /// </summary>
        /// <param name="id">League id.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>Return LeagueResponseModel</returns>
        Task<LeagueResponseModel> GetLeagueTableAsync(int id, CancellationToken cancellationToken);
    }
}
