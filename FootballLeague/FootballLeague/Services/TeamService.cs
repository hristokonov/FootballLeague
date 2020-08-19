using FootballLeague.Constants;
using FootballLeague.CustomExceptions;
using FootballLeague.Data;
using FootballLeague.Data.Models;
using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
    public class TeamService : ITeamService
    {
        private readonly ILeagueService _leagueService;
        private readonly ITeamDetailsService _teamDetailsService;
        private readonly FootballLeagueDbContext _context;

        public TeamService(ILeagueService leagueService, ITeamDetailsService teamDetailsService, FootballLeagueDbContext context)
        {
            _leagueService = leagueService;
            _context = context;
            _teamDetailsService = teamDetailsService;
        }

        /// <inheritdoc />
        public async Task<int> CreateTeamAsync(TeamRequestModel teamModel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await ValidateTeamExist(teamModel.Name, cancellationToken);
            await _leagueService.ValidateLeagueExistAsync(teamModel.LeagueId, cancellationToken);


            var team = new Team
            {
                Name = teamModel.Name,
                LeagueId = teamModel.LeagueId
            };

            _context.Teams.Add(team);

            await _context.SaveChangesAsync(cancellationToken);

            return team.Id;
        }

        /// <inheritdoc />
        public async Task DeleteTeamAsync(int teamId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var team = await GetTeamAsync(teamId, cancellationToken);

            _context.Teams.Remove(team);

            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<TeamResponseModel> GetTeamDetailsAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _teamDetailsService.GetTeamDetailsAsync(id, cancellationToken);
        }

        /// <inheritdoc />
        public async Task ValidateTeamExistInLeagueAsync(int teamId, int leagueId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var teamExists = await _context.Teams.Where(t => t.Id == teamId && t.LeagueId == leagueId)
                                       .AnyAsync(cancellationToken);

            if (!teamExists)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.TeamNotInLeague, teamId, leagueId));
            }
        }

        private async Task ValidateTeamExist(string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var teamExists = await _context.Teams
                                          .Where(t => t.Name == name)
                                          .AnyAsync(cancellationToken);

            if (teamExists)
            {
                throw new EntityAlreadyExistsException(string.Format(ErrorMessages.TeamExists, name));
            }
        }

        private async Task<Team> GetTeamAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var team = await _context.Teams
                 .Include(t => t.League)
                 .Include(t => t.HomeMatches)
                 .Include(t => t.AwayMatches)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (team == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.TeamNotFound, id));
            }

            return team;
        }
    }
}
