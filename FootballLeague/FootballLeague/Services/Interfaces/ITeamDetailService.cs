using FootballLeague.Models.Response;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface ITeamDetailService
    {
        /// <summary>
        /// Get team details.
        /// </summary>
        /// <param name="teamId">The team id.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>TeamResponseModel</returns>
        Task<TeamResponseModel> GetTeamDetailsAsync(int teamId, CancellationToken cancellationToken);
    }
}
