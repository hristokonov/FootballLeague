using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface IMatchService
    {
        /// <summary>
        /// Create a match
        /// </summary>
        /// <param name="matchModel">Match request model.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>Match id.</returns>
        Task<int> CreateMatchAsync(MatchRequestModel matchModel, CancellationToken cancellationToken);

        /// <summary>
        /// Play match
        /// </summary>
        /// <param name="matchModel">Match request model.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns></returns>
        Task PlayMatchAsync(MatchRequestModel matchModel, CancellationToken cancellationToken);

        /// <summary>
        /// Get all matches
        /// </summary>
        /// <returns>Collection of matches</returns>
        /// <param name="cancellationToken">The cancellationToken.</param>
        Task<IEnumerable<MatchResponseModel>> GetAllMatchesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get all matches of a team
        /// </summary>
        /// <param name="teamId">The id of the team.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>Collection of matches</returns>
        Task<IEnumerable<MatchResponseModel>> GetAllTeamMatchesAsync(int teamId, CancellationToken cancellationToken);

        /// <summary>
        /// Delete a match
        /// </summary>
        /// <param name="id">Match id.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns></returns>
        Task DeleteMatchAsync(int id, CancellationToken cancellationToken);
    }
}
