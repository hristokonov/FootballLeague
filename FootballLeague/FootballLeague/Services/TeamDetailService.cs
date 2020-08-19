using FootballLeague.Constants;
using FootballLeague.CustomExceptions;
using FootballLeague.Data;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
    public class TeamDetailService : ITeamDetailsService
    {
        private readonly FootballLeagueDbContext _context;

        public TeamDetailService(FootballLeagueDbContext context)
        {
            _context = context;
        }

        public async Task<TeamResponseModel> GetTeamDetailsAsync(int id, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                .Include(t => t.League)
                .Include(t => t.HomeMatches)
                .Include(t => t.AwayMatches)
                .Select(t => new TeamResponseModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    LeagueName = t.League.Name,
                    GoalsScored = t.GoalsScored,
                    GoalsConceded = t.GoalsConceded,
                    GoalDifference = t.GoalsScored - t.GoalsConceded,
                    Points = t.Points,
                    MatchesPlayed = t.HomeMatches.Count + t.AwayMatches.Count
                }
               )
               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (team == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.TeamNotFound, id));
            }

            return team;
        }
    }
}
